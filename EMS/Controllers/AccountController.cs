using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EMS.Services;
using Data.FormModels;
using EMS.Data.Entities.Emp;

namespace EMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly EmailService _emailService;
        private static string storedOtp;
        private Employee employee;

        public AccountController(UserManager<Employee> userManager,
                                 SignInManager<Employee> signInManager,
                                 EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
            {
                ViewBag.Error = "❌ Invalid email or password.";
                return View();
            }

            if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                Random random = new Random();
                string otp = random.Next(100000, 999999).ToString();

                string subject = "Your OTP Code";
                string messageBody = $"Your OTP Code is: {otp}";

                await _emailService.SendEmailAsync(email, subject, messageBody);

                HttpContext.Session.SetString("OTPVerified", "false");
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("GeneratedOTP", otp);

                return RedirectToAction("VerifyOTP");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

            //var employee = await _userManager.FindByEmailAsync(email);
            //if (employee == null)
            //{
            //    ModelState.AddModelError(string.Empty, "User not found.");
            //    return RedirectToAction("Login", "Account");
            //}


            //await _signInManager.SignInAsync(employee, isPersistent: false);

            //return RedirectToAction("Index", "Home");
        //}


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Employee
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

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        
        public IActionResult VerifyOTP()
        {
            ViewBag.Email = TempData["Email"];
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> VerifyOTP(string otp)
        {
            string email = HttpContext.Session.GetString("UserEmail");
            string storedOtp = HttpContext.Session.GetString("GeneratedOTP");

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            if (otp == storedOtp)
            {
                HttpContext.Session.SetString("OTPVerified", "true");

                var employee = await _userManager.FindByEmailAsync(email);
                if (employee == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("Login", "Account");
                }

                await _signInManager.SignInAsync(employee, isPersistent: false);

                HttpContext.Session.Remove("GeneratedOTP");

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "❌ Invalid OTP. Try again.";
                return View();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Account");
        }
    }
}
