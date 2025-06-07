using EMS.Data.Entities.Leaves;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Service.Interface
{
    public interface ILeaveRequestService
    {
        Task<IList<LeaveRequest>> FindAll(Expression<Func<LeaveRequest, bool>> expression = null, Func<IQueryable<LeaveRequest>, IOrderedQueryable<LeaveRequest>> orderBy = null,
            Func<IQueryable<LeaveRequest>, IIncludableQueryable<LeaveRequest, object>> includes = null);

        Task AddLeaveRequest(LeaveRequest leaveRequest);

        Task<LeaveRequest> GetLeaveRequestById(int id);

        Task UpdateLeaveRequest(LeaveRequest leaveRequest);

        Task RemoveLeaveRequest(LeaveRequest leaveRequest);
    }
}