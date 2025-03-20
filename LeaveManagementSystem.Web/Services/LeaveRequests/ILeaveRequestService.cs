using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<LeaveRequestReadOnlyVM> GetAllLeaveRequests();
        Task CancelLeaveRequewst(int leaveRequestId);
        Task ReviewLeaveRequest(ReviewLeaveRequestVM model);
        Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests();
    }
}