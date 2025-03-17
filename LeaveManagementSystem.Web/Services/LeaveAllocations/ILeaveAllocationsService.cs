using LeaveManagementSystem.Web.Models.LeaveAllocations;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string EmployeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocations(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
        Task EditAllocation(LeaveAllocationEditVM allocationEditVm);
    }
}
