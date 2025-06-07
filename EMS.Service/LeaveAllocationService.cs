using EMS.Data.Entities.Leaves;
using EMS.Repository;
using EMS.Service.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Service
{
    public class LeaveAllocationService : ILeaveAllocationService
    {
        private readonly IRepository<LeaveAllocation> _leaveAllocationRepository;

        public LeaveAllocationService(IRepository<LeaveAllocation> leaveAllocationRepository)
        {
            _leaveAllocationRepository = leaveAllocationRepository;
        }

        public async Task AddLeaveAllocation(LeaveAllocation leaveAllocation)
        {
            await _leaveAllocationRepository.Add(leaveAllocation);
        }

        public bool IsLeaveAllocationExists(string empId, int id, int period)
        {
            List<LeaveAllocation> leaveAllocationData = _leaveAllocationRepository.GetAll().Result.ToList();
            if (leaveAllocationData.Any(la => la.EmployeeId == empId && la.LeaveTypeId == id && la.Period == period))
            {
                return true;
            }

            return false;
        }

        public async Task DeleteLeaveAllocation(LeaveAllocation leaveAllocation)
        {
            if (leaveAllocation is not null)
            {
                await _leaveAllocationRepository.DeleteAsync(leaveAllocation);
            }
        }

        public async Task<LeaveAllocation> GetById(int id)
        {
            return await _leaveAllocationRepository.GetById(id);
        }

        public async Task UpdateLeaveAllocation(LeaveAllocation leaveAllocation)
        {
            await _leaveAllocationRepository.UpdateAsync(leaveAllocation);
        }

        public async Task<IList<LeaveAllocation>> FindAll(Expression<Func<LeaveAllocation, bool>> expression = null, Func<IQueryable<LeaveAllocation>, IOrderedQueryable<LeaveAllocation>> orderBy = null, Func<IQueryable<LeaveAllocation>, IIncludableQueryable<LeaveAllocation, object>> includes = null)
        {
            return await _leaveAllocationRepository.FindAll(expression, orderBy, includes);
        }

        public async Task<LeaveAllocation> Find(Expression<Func<LeaveAllocation, bool>> expression, Func<IQueryable<LeaveAllocation>, IIncludableQueryable<LeaveAllocation, object>> includes = null)
        {
            return await _leaveAllocationRepository.Find(expression, includes);
        }
    }
}
