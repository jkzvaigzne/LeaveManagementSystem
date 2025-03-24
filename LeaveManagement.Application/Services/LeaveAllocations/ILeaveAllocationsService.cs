using LeaveManagement.Application.Models.LeaveAllocations;

namespace LeaveManagement.Application.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string EmployeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocations(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
        Task EditAllocation(LeaveAllocationEditVM allocationEditVm);
        Task<LeaveAllocation> GetCurrentAllocation(int leaveTypesId, string employeeId);
    }
}
