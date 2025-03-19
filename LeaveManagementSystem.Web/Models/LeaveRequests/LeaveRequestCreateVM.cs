using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestCreateVM
    {
        [DisplayName("Start Date")]
        [Required]
        public DateOnly StartDate { get; set; }
        [DisplayName("End Date")]
        [Required]
        public DateOnly EndDate { get; set; }
        [DisplayName("Desired Leave Type")]
        [Required]
        public int LeaveTypeId { get; set; }
        [DisplayName("Additional Information")]
        public string? RequestComments { get; set; }

        public SelectList? LeaveTypes { get; set; }
    }
}