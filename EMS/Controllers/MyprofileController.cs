using Data.FormModels;
using EMS.Data;
using EMS.Data.Entities.Emp;
using Ems1.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers
{
    public class MyprofileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private EmpDbContext _empDbContext;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public MyprofileController(ILogger<HomeController> logger, EmpDbContext empDbContext, UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            _userManager = userManager;
            _empDbContext = empDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            var uid = _userManager.GetUserId(HttpContext.User);
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Employee user = _userManager.FindByIdAsync(uid).Result;
                if (user != null)
                {
                    RegisterViewModel model = new RegisterViewModel()
                    {
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Address = user.Address,
                        DateofBirth = user.DateofBirth,
                        DateofJoin = user.DateofJoin,
                        AadharNumber = user.AadharNumber,

                    };

                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegisterViewModel model)
        {
            var uid = _userManager.GetUserId(HttpContext.User);
            Employee user = _userManager.FindByIdAsync(uid).Result;
            if (user == null)
            {
                ViewBag.err = $"User with Id={model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Firstname = model.Firstname;
                user.Lastname = model.Lastname;
                user.Address = model.Address;
                user.DateofBirth = model.DateofBirth;
                user.DateofJoin = model.DateofJoin;
                user.AadharNumber = model.AadharNumber;

                user.UpdatedBy = _userManager.GetUserName(User);
                user.UpdatedDate = DateTime.Now;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyProfileDetail");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(model);

        }

        [HttpGet]
        public IActionResult MyProfileDetail()
        {
            var uid = _userManager.GetUserId(HttpContext.User);
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Employee user = _userManager.FindByIdAsync(uid).Result;
                if (user != null)
                {
                    RegisterViewModel model = new RegisterViewModel()
                    {
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Address = user.Address,
                        DateofBirth = user.DateofBirth,
                        DateofJoin = user.DateofJoin,
                        AadharNumber = user.AadharNumber,
                    };
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
