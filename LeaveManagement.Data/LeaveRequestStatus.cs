﻿namespace LeaveManagement.Data
{
    public class LeaveRequestStatus : BaseEntity
    {
        [StringLength(150)]
        public string Name { get; set; }
    }
}