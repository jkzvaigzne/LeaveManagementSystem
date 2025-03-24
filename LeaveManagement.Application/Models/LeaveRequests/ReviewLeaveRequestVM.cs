using LeaveManagement.Application.Models.LeaveAllocations;
using System.ComponentModel;

namespace LeaveManagement.Application.Models.LeaveRequests
{
    public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new EmployeeListVM();
        [DisplayName("Additional Information")]
        public string RequestComments { get; set; }
    }
}