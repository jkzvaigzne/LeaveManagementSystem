using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public class LeaveRequestService : ILeaveRequestService
    {
        public Task CancelLeaveRequewst(int leaveRequestId)
        {
            throw new NotImplementedException();
        }

        public Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveRequestListVM> GetAllLeaveRequests()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeLeaveRequestListVM> GetEmployeeLeaveRequests()
        {
            throw new NotImplementedException();
        }

        public Task ReviewLeaveRequest(ReviewLeaveRequestVM model)
        {
            throw new NotImplementedException();
        }
    }
}
