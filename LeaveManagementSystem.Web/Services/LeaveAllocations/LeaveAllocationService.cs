using AutoMapper;
using LeaveManagementSystem.Web.InvalidOperationExceptionHelpers;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationService(
        ApplicationDbContext _context,
        IHttpContextAccessor _httpContextAccessor,
        UserManager<ApplicationUser> _userManager,
        IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();


            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var period = await _context.Periods
                .Where(q => q.StartDate.Year <= currentDate.Year && q.EndDate.Year >= currentDate.Year)
                .OrderByDescending(q => q.EndDate)
                .FirstOrDefaultAsync();

            if (period == null)
            {
                throw new InvalidOperationException(InvalidOperationExceptionHelper.NoPeriodFoundForCurrentYear);
            }

            var monthsRemaining = (period.EndDate.Year - currentDate.Year) * 12 + period.EndDate.Month - currentDate.Month;

            foreach (var leaveType in leaveTypes)
            {
                // works but not best practice

                //var allocationExists = await AllocationExists(employeeId, period.Id, leaveType.Id);
                //if(allocationExists)
                //{
                //    continue;
                //}
                var accrualRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(accrualRate * monthsRemaining)
                };

                _context.Add(leaveAllocation);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId) 
                ? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User) 
                : await _userManager.FindByIdAsync(userId);

            var allocations = await GetAllocations(user.Id);
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            var employeeVM = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count()
            };

            return employeeVM;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Employee);
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());

            return employees;
        }

        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            var currentDate = DateTime.Now;
            var period = await _context.Periods
                .Where(q => q.StartDate.Year <= currentDate.Year && q.EndDate.Year >= currentDate.Year)
                .FirstOrDefaultAsync();

            if (period == null)
            {
                return new List<LeaveAllocation>();
            }

            var leaveAllocations = await _context.LeaveAllocation
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == userId && q.PeriodId == period.Id)
                .ToListAsync();

            return leaveAllocations;
        }

        private async Task<bool> AllocationExists(string userId, int periodId, int LeaveTypeId)
        {
            var exists = await _context.LeaveAllocation.AnyAsync(q =>
            q.EmployeeId == userId
            && q.LeaveTypeId == LeaveTypeId
            && q.PeriodId == periodId
            );

            return exists;
        }
    }
}