using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public class LeaveRequestService(IMapper _mapper,
        UserManager<ApplicationUser> _userManager,
        IHttpContextAccessor _httpContextAccessor,
        ApplicationDbContext _context) : ILeaveRequestService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequest.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;
            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
            var allocation = await _context.LeaveAllocation.
                FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId 
                && q.EmployeeId == leaveRequest.EmployeeId
                && q.PeriodId == period.Id
                );

            allocation.Days += numberOfDays;

            await _context.SaveChangesAsync();
        }
        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .ToListAsync();

            var aprovedLeaveRequestsCount = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved);
            var pendingLeaveRequestsCount = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending);
            var declinedLeaveRequestsCount = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined);

            var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType?.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
            }).ToList();

            var model = new EmployeeLeaveRequestListVM
            {
                ApprovedRequests = aprovedLeaveRequestsCount,
                PendingRequests = pendingLeaveRequestsCount,
                DeclinedRequests = declinedLeaveRequestsCount,
                TotalRequests = leaveRequests.Count,
                LeaveRequests = leaveRequestModels
            };

            return model;
        }
        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = user.Id;

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;

            _context.Add(leaveRequest);

            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocation.
                FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId 
                && q.EmployeeId == user.Id
                && q.PeriodId == period.Id
                );

            allocationToDeduct.Days -= numberOfDays;

            await _context.SaveChangesAsync();
        }

        public Task<LeaveRequestReadOnlyVM> GetAllLeaveRequests()
        {
            throw new NotImplementedException();
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var leaveRequests = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
              StartDate = q.StartDate,
              EndDate = q.EndDate,
              Id = q.Id,
              LeaveType = q.LeaveType?.Name,
              LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
              NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
            }).ToList();

            return model;
        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var currentDate = DateTime.Now; 
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;

            var allocation = await _context.LeaveAllocation.FirstAsync(q => q.LeaveTypeId 
            == model.LeaveTypeId 
            && q.EmployeeId == user.Id
            && q.PeriodId == period.Id
            );

            return allocation.Days < numberOfDays;
        }

        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            var leaveRequest = await _context.LeaveRequest.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId = approved 
                ? (int)LeaveRequestStatusEnum.Approved 
                : (int)LeaveRequestStatusEnum.Declined;

            leaveRequest.ReviewerId = user.Id;

            if(!approved)
            {
                var currentDate = DateTime.Now;
                var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
                var allocation = await _context.LeaveAllocation.
                FirstAsync(q => q.LeaveTypeId == leaveRequest.LeaveTypeId 
                && q.EmployeeId == leaveRequest.EmployeeId
                && q.PeriodId == period.Id);
                
                var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
                allocation.Days += numberOfDays;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequest = await _context.LeaveRequest
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == id);

            var user = await _userManager.FindByIdAsync(leaveRequest.EmployeeId);

            var model = new ReviewLeaveRequestVM
            {
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                Id = leaveRequest.Id,
                LeaveType = leaveRequest.LeaveType.Name,
                RequestComments = leaveRequest.RequestComments,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequest.Employee.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };

            return model;
        }
    }
}
