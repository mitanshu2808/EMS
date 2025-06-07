using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.FormModels;

namespace EMS.Data.FormModels.EmpSalary
{
    public class EmpSalaryVM
    {
        public int Id { get; set; }

        public RegisterViewModel? RequestingEmployee { get; set; }
        [Display(Name = "Employee Name")]
        public string? RequestingEmpId { get; set; }

        [Display(Name = "Salarydate ")]
        public DateTime Salarydate { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "GrossSalary")]
        public double GrossSalary { get; set; }

        [Required]
        [Display(Name = "TotalWorkingDays")]
        public int TotalWorkingDays { get; set; }

        [Required]
        [Display(Name = "LOPDays")]
        public int LOPDays { get; set; } = 0;

        [Required]
        [Display(Name = "PaidDays")]
        public int PaidDays { get; set; }

        [Required]
        [Display(Name = "BankName")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "AccountNumber")]
        public string AccountNumber { get; set; }
        
        [Required][Display(Name = "HRA")] 
        public double HRA { get; set; } = 0;

        [Required]
        [Display(Name = "ConveyanceAllowance")]
        public double ConveyanceAllowance { get; set; } = 0;

        [Required]
        [Display(Name = "MedicalAllowance")]
        public double MedicalAllowance { get; set; } = 0;

        [Required]
        [Display(Name = "SalaryAmount ")]
        public double SalaryAmount { get; set; } = 0;

        [Required]
        [Display(Name = "NetSalary ")]
        public double NetSalary { get; set; } = 0;

        [Required]
        [Display(Name = "EPF")]
        public double EPF { get; set; } = 0;

        [Required]
        [Display(Name = "ProfessionalTax")]
        public double ProfessionalTax { get; set; } = 0;

        [Required]
        [Display(Name = "LeaveDeduction")]
        public double LeaveDeduction { get; set; } = 0;

        [Required]
        [Display(Name = "DailySalary")]
        public double DailySalary { get; set; } = 0;

        [Required]
        [Display(Name = "TotalLeaves")]
        public double TotalLeaves { get; set; } = 0;

        [ForeignKey("CreatedbyId")]
        public RegisterViewModel? Createdby { get; set; }
        public string? CreatedbyId { get; set; }
    }
}