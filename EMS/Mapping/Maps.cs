using AutoMapper;
using Data.FormModels;
using EMS.Data.Entities.Emp;
using EMS.Data.Entities.Leaves;
using EMS.Data.FormModels.EmpLeave;
using EMS.Data.FormModels.EmpSalary;

namespace EMS.Mapping
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<LeaveType, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationVM>().ReverseMap();
            CreateMap<Employee, RegisterViewModel>().ReverseMap();
            CreateMap<LeaveAllocation, EditLeaveAllocationVM>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestVM>().ReverseMap();
            CreateMap<EmpSalary, EmpSalaryVM>().ReverseMap();
        }
    }
}
