using EMS.Data.Entities.Emp;
using EMS.Service.Interface;
using EMS.Repository;
using EMS.Data.Entities.Leaves;

namespace EMS.Service
{
    public class AmountService : IAmountService
    {
        private readonly IRepository<EmpSalary> _amountRepository;

        public AmountService(IRepository<EmpSalary> amountRepository)
        {
            _amountRepository = amountRepository;
        }

        public async Task<ICollection<EmpSalary>> GetSalaryForEmployee(string employeeid)
        {
            var leaveRequests = await _amountRepository.GetAll();
            return leaveRequests.Where(q => q.RequestingEmpId == employeeid).ToList();
        }

        public bool IsSalaryForEmployeeExist(string empId, int currentMonth)
        {
            List<EmpSalary> empSalaries = _amountRepository.GetAll().Result.ToList();
            if (empSalaries.Any(es => es.RequestingEmpId == empId && es.Salarydate.Month == currentMonth))
            {
                return true;
            }

            return false;
        }

        //public async Task<IList<EmpSalary>> FindAll(Expression<Func<EmpSalary, bool>> expression = null,
        //    Func<IQueryable<EmpSalary>, IOrderedQueryable<EmpSalary>> orderBy = null,
        //    Func<IQueryable<EmpSalary>, IIncludableQueryable<EmpSalary, object>> includes = null)
        //{
        //    IQueryable<EmpSalary> query = _db;

        //    if (expression != null)
        //    {
        //        query = query.Where(expression);
        //    }

        //    if (includes != null)
        //    {
        //        query = includes(query);
        //    }

        //    if (orderBy != null)
        //    {
        //        query = orderBy(query);
        //    }

        //    return await query.ToListAsync();
        //}

        public async Task AddEmployeeSalary(EmpSalary empSalary)
        {
            await _amountRepository.Add(empSalary);
        }

        public List<EmpSalary> FindAll()
        {
            return _amountRepository.GetAll().Result.ToList();
        }

        public async Task<EmpSalary> GetEmpSalary(int id)
        {
            return await _amountRepository.GetById(id);
        }

        public async Task RemoveEmpSalary(int id)
        {
            EmpSalary empSalary = await GetEmpSalary(id);
            await _amountRepository.DeleteAsync(empSalary);
            await _amountRepository.SaveChanges();
        }
    }
}
