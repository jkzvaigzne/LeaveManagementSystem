using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class LeaveTypeCreateVM
    {
        [Required]
        [Length(4, 150, ErrorMessage = "Your name should be 4-150 length.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 90)]
        [Display(Name = "Maximum Allocation of Days")]
        public int NumberOfDays { get; set; }
    }
}
