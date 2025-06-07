using EMS.Data.Entities.Emp;
using EMS.Data.Entities.Leaves;
using EMS.Repository;
using EMS.Service.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IRepository<LeaveRequest> _leaveRequestRepository;

        public LeaveRequestService(IRepository<LeaveRequest> leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<IList<LeaveRequest>> FindAll(Expression<Func<LeaveRequest, bool>> expression = null, Func<IQueryable<LeaveRequest>, IOrderedQueryable<LeaveRequest>> orderBy = null, Func<IQueryable<LeaveRequest>, IIncludableQueryable<LeaveRequest, object>> includes = null)
        {
            return await _leaveRequestRepository.FindAll(expression, orderBy, includes);
        }

        public async Task AddLeaveRequest(LeaveRequest leaveRequest)
        {
            await _leaveRequestRepository.Add(leaveRequest);
        }

        public async Task<LeaveRequest> GetLeaveRequestById(int id)
        {
            return await _leaveRequestRepository.GetById(id);
        }

        public async Task UpdateLeaveRequest(LeaveRequest leaveRequest)
        {
            await _leaveRequestRepository.UpdateAsync(leaveRequest);
        }

        public async Task RemoveLeaveRequest(LeaveRequest leaveRequest)
        {
            await _leaveRequestRepository.DeleteAsync(leaveRequest);
            await _leaveRequestRepository.SaveChanges();
        }
    }
}