using AutoMapper;
using Data.FormModels;
using EMS.Data.Entities.Emp;
using EMS.Data.Entities.Leaves;
using EMS.Data.FormModels.EmpLeave;
using EMS.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers.Leave
{
    public class LeaveAllocationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveAllocationService _leaveAllocationService;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationController(IMapper mapper, UserManager<Employee> userManager, ILeaveTypeService leaveTypeService
                , ILeaveAllocationService leaveAllocationService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _leaveTypeService = leaveTypeService;
            _leaveAllocationService = leaveAllocationService;
        }

        public async Task<ActionResult> ListEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            var model = _mapper.Map<List<RegisterViewModel>>(employees);
            return View(model);
        }

        public async Task<ActionResult> LeaveAllocationDetail(string id)
        {
            var employee = _mapper.Map<RegisterViewModel>(await _userManager.FindByIdAsync(id));
            int period = DateTime.Now.Year;
            var records = await _leaveAllocationService.FindAll(
                expression: q => q.EmployeeId == id && q.Period == period, includes: q => q.Include(x => x.LeaveType));

            List<LeaveAllocationVM> allocations = _mapper.Map<List<LeaveAllocationVM>>(records);

            ViewAllocationsVM model = new ViewAllocationsVM
            {
                Employee = employee,
                LeaveAllocations = allocations
            };

            return View(model);
        }

        public async Task<ActionResult> EditLeaveAllocation(int id)
        {
            LeaveAllocation leaveAllocation = await _leaveAllocationService.Find(q => q.Id == id,
                includes: q => q.Include(x => x.Employee).Include(x => x.LeaveType));
            EditLeaveAllocationVM model = _mapper.Map<EditLeaveAllocationVM>(leaveAllocation);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditLeaveAllocation(EditLeaveAllocationVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                LeaveAllocation record = await _leaveAllocationService.GetById(model.Id);
                record.NumberOfDays = model.NumberOfDays;
                record.UpdatedDate = DateTime.Now;
                record.UpdatedBy = _userManager.GetUserName(User);
                await _leaveAllocationService.UpdateLeaveAllocation(record);

                return RedirectToAction(nameof(LeaveAllocationDetail), new { id = model.EmployeeId });
            }
            catch
            {
                return View(model);
            }
        }
    }
}
