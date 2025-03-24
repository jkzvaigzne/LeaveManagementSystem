using LeaveManagement.Application.Models.LeaveRequests;

namespace LeaveManagement.Application.Services.LeaveRequests
{
    public interface ILeaveRequestService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task CancelLeaveRequest(int leaveRequestId);
        Task ReviewLeaveRequest(int leaveRequestId, bool approved);
        Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests();
        Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests();
        Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id);
    }
}