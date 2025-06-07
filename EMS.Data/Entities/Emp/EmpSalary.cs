using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Data.Entities.Emp
{
    public class EmpSalary : BaseEntity
    {
        [ForeignKey("RequestingEmpId")]
        public Employee? RequestingEmployee { get; set; }
        public string? RequestingEmpId { get; set; }
        public DateTime Salarydate { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public double GrossSalary { get; set; }
        public int TotalWorkingDays { get; set; }
        public int LOPDays { get; set; }
        public int PaidDays { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public double HRA { get; set; }
        public double ConveyanceAllowance { get; set; }
        public double MedicalAllowance { get; set; }
        public double SalaryAmount { get; set; }
        public double NetSalary { get; set; }
        public double EPF { get; set; }
        public double ProfessionalTax { get; set; }

        [ForeignKey("CreatedbyId")]
        public Employee? Createdby { get; set; }
        public string? CreatedbyId { get; set; }
    }
}
