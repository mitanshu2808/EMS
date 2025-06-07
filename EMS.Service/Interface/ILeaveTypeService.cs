using EMS.Data.Entities.Leaves;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Service.Interface
{
    public interface ILeaveTypeService
    {
        Task AddLeaveType(LeaveType leaveType);
        Task UpdateLeaveType(LeaveType leaveType);
        Task<LeaveType> GetLeaveTypeById(int id);
        Task DeleteLeaveType(LeaveType leaveType);

        Task<List<LeaveType>> GetAllLeaveTypes();

        Task<LeaveType> Find(Expression<Func<LeaveType, bool>> expression, Func<IQueryable<LeaveType>, 
            IIncludableQueryable<LeaveType, object>> includes = null);
    }
}
