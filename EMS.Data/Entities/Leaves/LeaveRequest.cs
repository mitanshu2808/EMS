using System.ComponentModel.DataAnnotations.Schema;
using Data.FormModels;
using EMS.Data.Entities.Emp;

namespace EMS.Data.Entities.Leaves
{
    public class LeaveRequest : BaseEntity
    {
        [ForeignKey("RequestingEmpId")]
        public Employee? RequestingEmployee { get; set; }
        public string? RequestingEmpId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("LeaveTypeId")]
        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime DateRequested { get; set; }
        public string? RequestComments { get; set; } = string.Empty;
        public DateTime DateActioned { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }

        [ForeignKey("ApprovedById")]
        public RegisterViewModel? ApprovedBy { get; set; }
        public string? ApprovedById { get; set; } = string.Empty;
    }
}
