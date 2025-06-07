using AutoMapper;
using Data.FormModels;
using EMS.Data.Entities.Emp;
using EMS.Data.Entities.Leaves;
using EMS.Data.FormModels.EmpLeave;
using EMS.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    public class EmployeController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveAllocationService _leaveAllocationService;
        private readonly IAmountService _amountService;
        private readonly IMapper _mapper;

        public EmployeController(UserManager<Employee> userManager, SignInManager<Employee> signInManager,
                               ILeaveTypeService leaveTypeService, ILeaveAllocationService leaveAllocationService, IMapper mapper, IAmountService amountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _leaveTypeService = leaveTypeService;
            _leaveAllocationService = leaveAllocationService;
            _mapper = mapper;
            _amountService = amountService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (model is not null)
            {
                Employee emp = new Employee
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Address = model.Address,
                    DateofBirth = model.DateofBirth,
                    DateofJoin = model.DateofJoin,
                    AadharNumber = model.AadharNumber,
                    UserName = model.Email,
                    Email = model.Email,
                    CreatedBy = _userManager.GetUserName(User),
                    CreatedDate = DateTime.Now,
                    EmailConfirmed = true,
                };

                var result = await _userManager.CreateAsync(emp, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(emp, "Employee");
                    List<LeaveType> leaveTypes = _leaveTypeService.GetAllLeaveTypes().Result;
                    int period = DateTime.Now.Year;
                    foreach (LeaveType leave in leaveTypes)
                    {
                        if (_leaveAllocationService.IsLeaveAllocationExists(emp.Id, leave.Id, period))
                            continue;

                        LeaveAllocationVM allocation = new LeaveAllocationVM
                        {
                            DateCreated = DateTime.Now,
                            EmployeeId = emp.Id,
                            LeaveTypeId = leave.Id,
                            NumberOfDays = leave.DefaultDays,
                            Period = DateTime.Now.Year
                        };

                        var leaveallocation = _mapper.Map<LeaveAllocation>(allocation);
                        leaveallocation.CreatedDate = DateTime.Now;
                        leaveallocation.CreatedBy = _userManager.GetUserName(User);
                        await _leaveAllocationService.AddLeaveAllocation(leaveallocation);
                    }

                    return RedirectToAction("EmployeList", "Employe");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid user data");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeList()
        {
            List<RegisterViewModel> employees = new List<RegisterViewModel>();
            IList<Employee> items = await _userManager.GetUsersInRoleAsync("Employee");
            foreach (Employee i in items)
            {
                employees.Add(new RegisterViewModel { Id = i.Id, Firstname = i.Firstname, Lastname = i.Lastname, Email = i.Email, Address = i.Address, AadharNumber = i.AadharNumber, DateofBirth = i.DateofBirth, DateofJoin = i.DateofJoin });
            }

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Employee user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    RegisterViewModel model = new RegisterViewModel()
                    {
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Email = user.Email,
                        Address = user.Address,
                        DateofBirth = user.DateofBirth,
                        DateofJoin = user.DateofJoin,
                        AadharNumber = user.AadharNumber,
                    };

                    return View(model);
                }
            }

            return RedirectToAction("EmployeList");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(RegisterViewModel employee)
        {
            Employee user = await _userManager.FindByIdAsync(employee.Id);
            user.Firstname = employee.Firstname;
            user.Lastname = employee.Lastname;
            user.Email = employee.Email;
            user.UserName = employee.Email;
            user.Address = employee.Address;
            user.DateofBirth = employee.DateofBirth;
            user.DateofJoin = employee.DateofJoin;
            user.AadharNumber = employee.AadharNumber;
            user.UpdatedBy = _userManager.GetUserName(User);
            user.UpdatedDate = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("EmployeList");
            }

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            Employee user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                var empSalary = await _amountService.GetSalaryForEmployee(user.Id);
                if(empSalary is not null && empSalary.Any())
                {
                    foreach (var sal in empSalary)
                    {
                        await _amountService.RemoveEmpSalary(sal.Id);
                    }
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("EmployeList");
                }
                else
                {
                    foreach (var er in result.Errors)
                        ModelState.AddModelError("", er.Description);
                }
            }

            return View();
        }
    }
}
