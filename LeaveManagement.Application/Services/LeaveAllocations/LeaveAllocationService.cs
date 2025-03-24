using AutoMapper;
using LeaveManagement.Application.Models.LeaveAllocations;
using LeaveManagement.Application.Services.Periods;
using LeaveManagement.Application.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.Application.Services.LeaveAllocations
{
    public class LeaveAllocationService(
        ApplicationDbContext _context,
        IMapper _mapper,
        IPeriodsService _periodsService,
        IUserService _userService) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();


            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var period = await _periodsService.GetCurrentPeriod();

            if (period == null)
            {
                throw new InvalidOperationException(InvalidOperationExceptionHelper.NoPeriodFoundForCurrentYear);
            }

            var monthsRemaining = (period.EndDate.Year - currentDate.Year) * 12 + period.EndDate.Month - currentDate.Month;

            foreach (var leaveType in leaveTypes)
            {
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
                ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);

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
            var users = await _userService.GetEmployees();
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());

            return employees;
        }
        public async Task<LeaveAllocationEditVM> GetEmployeeAllocations(int allocationId)
        {
            var allocation = await _context.LeaveAllocation
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
        }

        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVm)
        {
            var leaveAllocation = await GetEmployeeAllocations(allocationEditVm.Id);

            if (leaveAllocation == null)
            {
                throw new Exception(InvalidOperationExceptionHelper.LeaveAllocationRecordNotExists);
            }

            leaveAllocation.Days = allocationEditVm.Days;
            await _context.LeaveAllocation
                .Where(q => q.Id == allocationEditVm.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, allocationEditVm.Days));
        }

        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {
            var period = await _periodsService.GetCurrentPeriod();

            var exists = await _context.LeaveAllocation
                .AnyAsync(x => x.LeaveTypeId == leaveTypeId
                && x.EmployeeId == employeeId
                && x.PeriodId == period.Id);

            if (!exists)
            {
                return null;
            }

            var allocation = await _context.LeaveAllocation
                .SingleAsync(x => x.LeaveTypeId == leaveTypeId
                && x.EmployeeId == employeeId
                && x.PeriodId == period.Id);

            return allocation;
        }
        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            var period = await _periodsService.GetCurrentPeriod();

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
