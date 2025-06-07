using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ems1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private EmpDbContext _empDbContext;
        public HomeController(ILogger<HomeController> logger, EmpDbContext empDbContext)
        {
            _logger = logger;
            _empDbContext = empDbContext;
        }

        [Authorize(Roles = "Admin,Superadmin,Employee")]
        public IActionResult Index()
        {
            var eventList = _empDbContext.Events.ToList();
            return View(eventList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}