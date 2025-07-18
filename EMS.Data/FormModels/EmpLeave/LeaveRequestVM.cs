﻿using Data.FormModels;
using EMS.Data.Entities.Emp;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EMS.Data.FormModels.EmpLeave
{
    public class LeaveRequestVM
    {
        public int Id { get; set; }
        public Employee? RequestingEmployee { get; set; }

        [Display(Name = "Employee Name")]
        public string RequestingEmpId { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public LeaveTypeVM? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        [Display(Name = "Date Actioned")]
        public DateTime DateActioned { get; set; }

        [Display(Name = "Approval State")]
        public bool? Approved { get; set; }

        public RegisterViewModel? ApprovedBy { get; set; }

        [Display(Name = "Approver Name")]
        public string? ApprovedById { get; set; } = string.Empty;

        public bool Cancelled { get; set; }

        [Display(Name = "Employee Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; } = string.Empty;
    }

    public class AdminLeaveRequestViewVM
    {
        [Display(Name = "Total Number Of Requests")]
        public int TotalRequests { get; set; }

        [Display(Name = "Approved Requests")]
        public int ApprovedRequests { get; set; }

        [Display(Name = "Pending Requests")]
        public int PendingRequests { get; set; }

        [Display(Name = "Rejected Requests")]
        public int RejectedRequests { get; set; }

        public List<LeaveRequestVM>? LeaveRequests { get; set; }
    }

    public class CreateLeaveRequestVM
    {

        [Display(Name = "Start Date")]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem>? LeaveTypes { get; set; }

        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; } = string.Empty;
    }

    public class EmployeeLeaveRequestViewVM
    {
        public List<LeaveAllocationVM>? LeaveAllocations { get; set; }
        public List<LeaveRequestVM>? LeaveRequests { get; set; }
    }
}
