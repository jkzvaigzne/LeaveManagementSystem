using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public class LeaveRequestService(IMapper _mapper,
        UserManager<ApplicationUser> _userManager,
        IHttpContextAccessor _httpContextAccessor,
        ApplicationDbContext _context) : ILeaveRequestService
    {
        public Task CancelLeaveRequewst(int leaveRequestId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = user.Id;

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;

            _context.Add(leaveRequest);

            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocation.
                FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId && q.EmployeeId == user.Id);

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
              NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
            }).ToList();

            return model;
        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;

            var allocation = await _context.LeaveAllocation.FirstAsync(q => q.LeaveTypeId 
            == model.LeaveTypeId && q.EmployeeId == user.Id);

            return allocation.Days < numberOfDays;
        }

        public Task ReviewLeaveRequest(ReviewLeaveRequestVM model)
        {
            throw new NotImplementedException();
        }
    }
}
