using EMS.Data.Entities.Emp;

namespace EMS.Service.Interface
{
    public interface IAmountService
    {
        Task AddEmployeeSalary(EmpSalary empSalary);

        Task<EmpSalary> GetEmpSalary(int id);

        bool IsSalaryForEmployeeExist(string empId, int currentMonth);

        Task RemoveEmpSalary(int id);

        List<EmpSalary> FindAll();

        Task<ICollection<EmpSalary>> GetSalaryForEmployee(string employeeid);
    }
}
