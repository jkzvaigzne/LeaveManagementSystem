namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string EmployeeId);
        Task<List<LeaveAllocation>> GetAllocations();
    }
}
