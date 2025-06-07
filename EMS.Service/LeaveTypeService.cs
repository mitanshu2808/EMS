using EMS.Data.Entities.Emp;
using EMS.Data.Entities.Leaves;
using EMS.Repository;
using EMS.Service.Interface;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EMS.Service
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly IRepository<LeaveType> _leaveTypeRepository;

        public LeaveTypeService(IRepository<LeaveType> leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task AddLeaveType(LeaveType leaveType)
        {
            await _leaveTypeRepository.Add(leaveType);
        }

        public async Task<LeaveType> GetLeaveTypeById(int id)
        {
            return await _leaveTypeRepository.GetById(id);
        }

        public async Task UpdateLeaveType(LeaveType leaveType)
        {
            await _leaveTypeRepository.UpdateAsync(leaveType);
        }

        public async Task DeleteLeaveType(LeaveType leaveType)
        {
            //LeaveType leaveType = await GetLeaveTypeById(id);
            if(leaveType is not null)
            {
                await _leaveTypeRepository.DeleteAsync(leaveType);
            }
        }

        public async Task<List<LeaveType>> GetAllLeaveTypes()
        {
            return await _leaveTypeRepository.GetAll();
        }

        public async Task<LeaveType> Find(Expression<Func<LeaveType, bool>> expression, Func<IQueryable<LeaveType>, IIncludableQueryable<LeaveType, object>> includes = null)
        {
            return await _leaveTypeRepository.Find(expression, includes);
        }
    }
}
