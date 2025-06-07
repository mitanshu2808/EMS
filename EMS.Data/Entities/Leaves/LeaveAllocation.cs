using System.ComponentModel.DataAnnotations.Schema;
using EMS.Data.Entities.Emp;

namespace EMS.Data.Entities.Leaves
{
    public class LeaveAllocation : BaseEntity
    {
        public int NumberOfDays { get; set; }

        public DateTime DateCreated { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        public string? EmployeeId { get; set; }

        [ForeignKey("LeaveTypeId")]
        public LeaveType? LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        public int Period { get; set; }
    }
}