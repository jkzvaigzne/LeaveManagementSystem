using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<EmployeeLeaveRequestListVM> GetEmployeeLeaveRequests();
        Task<LeaveRequestListVM> GetAllLeaveRequests();
        Task CancelLeaveRequewst(int leaveRequestId);
        Task ReviewLeaveRequest(ReviewLeaveRequestVM model);
    }
}