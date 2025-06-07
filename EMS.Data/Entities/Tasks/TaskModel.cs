using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EMS.Data.Entities.Emp;// Assuming Employee entity is in this namespace

namespace EMS.Data.Entities.Tasks
{
    public class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }  // Auto-incrementing primary key

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }  // Task title

        public string Description { get; set; }  // Task description (optional)

        [Required]
        public DateTime Deadline { get; set; }  // Task deadline

        //public string EmployeeId { get; set; }

        ////[ForeignKey("EmployeeId")]
        //public Employee? Employee { get; set; } // Navigation Property for Employee
        [Required]

        public string AssignedBy { get; set; }  // Admin ID who assigned the task

        [Required]
        public string AssignedTo { get; set; }
        public DateTime DueDate { get; set; }  // Due date for completion

        public bool IsCompleted { get; set; } = false;  // Default: false

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";  // Default: Pending
    }
}

