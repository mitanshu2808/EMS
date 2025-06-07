using AutoMapper;
using EMS.Data.Entities.Emp;
using EMS.Data.Entities.Leaves;
using EMS.Data.FormModels.EmpLeave;
using EMS.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers.Leave
{
    [Authorize(Roles = "Admin,Superadmin")]
    public class LeaveTypeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveAllocationService _leaveAllocationService;

        public LeaveTypeController(UserManager<Employee> userManager, IMapper mapper, ILeaveTypeService leaveTypeService,
                ILeaveAllocationService leaveAllocationService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _leaveTypeService = leaveTypeService;
            _leaveAllocationService = leaveAllocationService;
        }

        public async Task<IActionResult> LeaveTypeList()
        {
            List<LeaveType> leavetypes = await _leaveTypeService.GetAllLeaveTypes();
            List<LeaveTypeVM> model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes.ToList());
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if(_leaveTypeService.GetAllLeaveTypes().Result.Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "Leave Type already exist...");
                    return View(model);
                }    

                LeaveType leaveType = _mapper.Map<LeaveType>(model);
                leaveType.CreatedDate = DateTime.Now;
                leaveType.CreatedBy = _userManager.GetUserName(User);
                await _leaveTypeService.AddLeaveType(leaveType);
                await SetLeaveAllocation(leaveType.Id);
                return RedirectToAction("LeaveTypeList");
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        public async Task<ActionResult> SetLeaveAllocation(int id)
        {
            LeaveType leavetype = await _leaveTypeService.GetLeaveTypeById(id);
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            var period = DateTime.Now.Year;
            foreach (var emp in employees)
            {
                if (_leaveAllocationService.IsLeaveAllocationExists(emp.Id, id, period))
                    continue;

                LeaveAllocationVM allocation = new LeaveAllocationVM
                {
                    DateCreated = DateTime.Now,
                    EmployeeId = emp.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leavetype.DefaultDays,
                    Period = DateTime.Now.Year
                };

                var leaveallocation = _mapper.Map<LeaveAllocation>(allocation);
                leaveallocation.CreatedDate = DateTime.Now;
                leaveallocation.CreatedBy = _userManager.GetUserName(User);
                await _leaveAllocationService.AddLeaveAllocation(leaveallocation);
            }

            return RedirectToAction("AllocateLeave");
        }

        public async Task<ActionResult> EditLeaveType(int id)
        {
            var leaveType = await _leaveTypeService.GetLeaveTypeById(id);
            if (leaveType is null)
            {
                return NotFound();
            }

            LeaveTypeVM leaveTypeVM = _mapper.Map<LeaveTypeVM>(leaveType);
            return View(leaveTypeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditLeaveType(LeaveTypeVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                LeaveType leaveType = _mapper.Map<LeaveType>(model);
                await _leaveTypeService.UpdateLeaveType(leaveType);
                leaveType.UpdatedDate = DateTime.Now;
                leaveType.UpdatedBy = _userManager.GetUserName(User);

                return RedirectToAction("LeaveTypeList");
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLeaveType(int id)
        {
            try
            {
                string empId = _userManager.GetUserId(User);
                if (!_leaveAllocationService.IsLeaveAllocationExists(empId, id, DateTime.Now.Year))
                {
                    LeaveType leavetype = await _leaveTypeService.GetLeaveTypeById(id);
                    if (leavetype == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        await _leaveTypeService.DeleteLeaveType(leavetype);
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Something Went Wrong...", ex.Message);
            }

            return RedirectToAction("LeaveTypeList");
        }
    }
}