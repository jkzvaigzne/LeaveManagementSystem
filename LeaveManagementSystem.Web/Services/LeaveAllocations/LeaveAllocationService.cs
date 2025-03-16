using AutoMapper;
using LeaveManagementSystem.Web.InvalidOperationExceptionHelpers;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
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
            // Get all the leave types
            var leaveTypes = await _context.LeaveTypes.ToListAsync();

            // Get the current date as DateOnly for consistent comparison with Period's DateOnly fields
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            // Get the current period based on the year
            var period = await _context.Periods
                .Where(q => q.StartDate.Year <= currentDate.Year && q.EndDate.Year >= currentDate.Year)
                .OrderByDescending(q => q.EndDate)
                .FirstOrDefaultAsync();

            if (period == null)
            {
                throw new InvalidOperationException(InvalidOperationExceptionHelper.NoPeriodFoundForCurrentYear);
            }

            // Calculate the remaining months in the period, considering the year span
            var monthsRemaining = (period.EndDate.Year - currentDate.Year) * 12 + period.EndDate.Month - currentDate.Month;

            // Iterate through each leave type and allocate the leave
            foreach (var leaveType in leaveTypes)
            {
                // Calculate the accrual rate (days per month)
                var accrualRate = decimal.Divide(leaveType.NumberOfDays, 12);

                // Calculate the leave allocation for the remaining months
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(accrualRate * monthsRemaining)
                };

                // Add the allocation to the context
                _context.Add(leaveAllocation);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveAllocation>> GetAllocations()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var leaveAllocations = await _context.LeaveAllocation
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocation()
        {
            var allocations = await GetAllocations();
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var employeeVM = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList
            };

            return employeeVM;
        }
    }
}
