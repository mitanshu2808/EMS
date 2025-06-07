using EMS.Data.Entities.Leaves;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Service.Interface
{
    public interface ILeaveAllocationService
    {
        public bool IsLeaveAllocationExists(string empId, int id, int period);

        Task AddLeaveAllocation(LeaveAllocation leaveAllocation);

        Task DeleteLeaveAllocation(LeaveAllocation leaveAllocation);

        Task<LeaveAllocation> GetById(int id);

        Task UpdateLeaveAllocation(LeaveAllocation leaveAllocation);

        Task<LeaveAllocation> Find(Expression<Func<LeaveAllocation, bool>> expression, Func<IQueryable<LeaveAllocation>, IIncludableQueryable<LeaveAllocation, object>> includes = null);

        Task<IList<LeaveAllocation>> FindAll(Expression<Func<LeaveAllocation, bool>> expression = null, Func<IQueryable<LeaveAllocation>, IOrderedQueryable<LeaveAllocation>> orderBy = null, Func<IQueryable<LeaveAllocation>, IIncludableQueryable<LeaveAllocation, object>> includes = null);
    }
}
