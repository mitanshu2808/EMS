using AutoMapper;
using EMS.Data.Entities.Emp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EMS.Data;
using EMS.Service.Interface;
using Microsoft.EntityFrameworkCore;
using EMS.Data.Entities.Leaves;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Service;
using EMS.Data.FormModels.EmpLeave;

namespace EMS.Controllers.Leave
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly ILeaveAllocationService _leaveAllocationService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly UserManager<Employee> _userManager;

        public LeaveRequestController(IEmailSender emailSender, IMapper mapper, UserManager<Employee> userManager, 
            ILeaveRequestService leaveRequestService, ILeaveAllocationService leaveAllocationService, ILeaveTypeService leaveTypeService)
        {
            _emailSender = emailSender;
            _mapper = mapper;
            _userManager = userManager;
            _leaveRequestService = leaveRequestService;
            _leaveAllocationService = leaveAllocationService;
            _leaveTypeService = leaveTypeService;
        }

        [Authorize(Roles = "Admin,Superadmin")]
        public async Task<IActionResult> LeaveRequestList()
        {
            var leaveRequests = await _leaveRequestService.FindAll(
                includes: q => q.Include(x => x.RequestingEmployee).Include(x => x.LeaveType));

            List<LeaveRequestVM> leaveRequstsModel = _mapper.Map<List<LeaveRequestVM>>(leaveRequests);
            AdminLeaveRequestViewVM model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequstsModel.Count,
                ApprovedRequests = leaveRequstsModel.Count(q => q.Approved == true),
                PendingRequests = leaveRequstsModel.Count(q => q.Approved == null),
                RejectedRequests = leaveRequstsModel.Count(q => q.Approved == false),
                LeaveRequests = leaveRequstsModel
            };

            return View(model);
        }

        [Authorize(Roles = "Employee")]
        public async Task<ActionResult> MyLeave()
        {
            Employee employee = await _userManager.GetUserAsync(User);
            var employeeid = employee.Id;
            var employeeAllocations = await _leaveAllocationService.FindAll(expression: q => q.EmployeeId == employeeid, includes: q => q.Include(x => x.LeaveType));

            var employeeRequests = await _leaveRequestService.FindAll(expression: q => q.RequestingEmpId == employeeid,
                includes: q => q.Include(x => x.RequestingEmployee).Include(x => x.LeaveType));

            var employeeAllocationsModel = _mapper.Map<List<LeaveAllocationVM>>(employeeAllocations);
            var employeeRequestsModel = _mapper.Map<List<LeaveRequestVM>>(employeeRequests);

            var model = new EmployeeLeaveRequestViewVM
            {
                LeaveAllocations = employeeAllocationsModel,
                LeaveRequests = employeeRequestsModel
            };

            return View(model);

        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var leaveTypes = await _leaveTypeService.GetAllLeaveTypes();
            var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString(),
            });

            CreateLeaveRequestVM model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItems
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestVM model)
        {
            try
            {
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if(model.StartDate.HasValue && model.EndDate.HasValue)
                {
                    startDate = model.StartDate.Value;
                    endDate = model.EndDate.Value;
                }

                List<LeaveType> leaveTypes = await _leaveTypeService.GetAllLeaveTypes();
                Employee employee = await _userManager.GetUserAsync(User);
                int period = DateTime.Now.Year;
                LeaveAllocation allocation = await _leaveAllocationService.Find(expression: q => q.EmployeeId == employee.Id
                                                    && q.Period == period
                                                    && q.LeaveTypeId == model.LeaveTypeId);
                int daysRequested = (endDate.Day - startDate.Day) + 1;
                var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });

                model.LeaveTypes = leaveTypeItems;
                if (allocation == null)
                {
                    ModelState.AddModelError("", "You Have No Days Left");
                }
                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start Date cannot be further in the future than the End Date");
                }
                if (daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "You Do Not Sufficient Days For This Request");
                }

                LeaveRequestVM leaveRequestModel = new LeaveRequestVM
                {
                    RequestingEmpId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    ApprovedById = null,
                    ApprovedBy = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId,
                    RequestComments = model.RequestComments
                };

                LeaveRequest leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);
                leaveRequest.CreatedDate = DateTime.Now;
                leaveRequest.CreatedBy = _userManager.GetUserName(User);
                await _leaveRequestService.AddLeaveRequest(leaveRequest);

                // Send Email to supervisor and requesting user
                await _emailSender.SendEmailAsync("admin@localhost.com", "New Leave Request",
                    $"Please review this leave request. <a href='UrlOfApp/{leaveRequest.Id}'>Click Here</a>");

                return RedirectToAction("MyLeave");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelRequest(int id)
        {
            LeaveRequest leaveRequest = await _leaveRequestService.GetLeaveRequestById(id);
            if (leaveRequest is not null)
            {
                await _leaveRequestService.RemoveLeaveRequest(leaveRequest);
            }

            return RedirectToAction("MyLeave");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLeaveStatus(int id,bool status)
        {
            LeaveRequest leaveRequest = await _leaveRequestService.GetLeaveRequestById(id);
            if (leaveRequest is not null)
            {
                leaveRequest.Approved = status;
                await _leaveRequestService.UpdateLeaveRequest(leaveRequest);
            }

            return RedirectToAction("LeaveRequestList");
        }

        [HttpGet]
        public async Task<IActionResult> LeaveDetails(int id)
        {
            var leaveRequests = await _leaveRequestService.FindAll(
                includes: q => q.Include(x => x.RequestingEmployee).Include(x => x.LeaveType));

            if(leaveRequests is not null)
            {
                LeaveRequest leaveRequest = leaveRequests.Where(x => x.Id == id).FirstOrDefault();

                LeaveRequestVM leaveRequstsModel = _mapper.Map<LeaveRequestVM>(leaveRequest);
                return View(leaveRequstsModel);
            }

            return RedirectToAction("Index","Home");
        }
    }
}
