﻿using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class LeaveTypeCreateVM
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int NumberOfDays { get; set; }
    }
}
