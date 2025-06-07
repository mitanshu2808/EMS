using System.Security.Claims;
using System.Threading.Tasks;
using EMS.Constants;
using EMS.Data;
using EMS.Data.Entities.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers
{
    public class TaskController : Controller
    {
        private readonly EmpDbContext _context;
        private readonly ILogger<TaskController> _logger;
        private readonly UserManager<Employee> _userManager;
        public TaskController(EmpDbContext context, UserManager<Employee> userManager, ILogger<TaskController> logger)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        //GET: List all tasks(Admin View)
        public async Task<IActionResult> Read()
        {
            var tasksAsQueryable = _context.Tasks.AsQueryable();
            if (User.IsInRole(UserRoles.Employee))
            {
                var UserId = _userManager.GetUserId(HttpContext.User);
                tasksAsQueryable = tasksAsQueryable.Where(x => x.AssignedTo == UserId);
            }

            ViewBag.Employees = _context.Employees.ToList();
            return View(await tasksAsQueryable.ToListAsync());
        }

        // GET: Create Task Form
        [HttpGet]
        public IActionResult Create()
        {
            TaskModel task = new();
            task.AssignedBy = _userManager.GetUserId(HttpContext.User);//userid of logged in user
            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Firstname");
            return View(task);
        }

        // POST: Save New Task
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                int result = await _context.SaveChangesAsync();

                if (result > 0) // If task is saved successfully
                {
                    _logger.LogInformation($"✅ Task '{task.Title}' assigned to {task.AssignedTo} was saved successfully.");
                    Console.WriteLine($"✅ Task '{task.Title}' assigned to {task.AssignedTo} was saved successfully.");
                }
                else // If saving failed
                {
                    _logger.LogError($"❌ Task '{task.Title}' could not be saved.");
                    Console.WriteLine($"❌ Task '{task.Title}' could not be saved.");
                }
                return RedirectToAction("Read");
            }
            else
            {
                _logger.LogWarning("⚠️ Invalid task data! Please check your inputs.");
                Console.WriteLine("⚠️ Invalid task data! Please check your inputs.");
            }

            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Firstname");
            return View(task);
        }

        // GET: Show the edit form with existing task details
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(); // If task doesn't exist, return 404
            }

            ViewBag.Employees = new SelectList(_context.Employees, "Id", "Firstname");
            return View(task); // Pass task to Edit.cshtml
        }

        // POST: Save the updated task data
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskModel updatedTask)
        {
            if (id != updatedTask.TaskId)
            {
                return BadRequest(); // Ensure correct task ID
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Employees = new SelectList(_context.Employees, "Id", "Firstname");
                return View(updatedTask);
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            // Update task details
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Deadline = updatedTask.Deadline;
            task.AssignedTo = updatedTask.AssignedTo;
            task.Status = updatedTask.Status;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Read"); // Redirect back to task list
        }


        // GET: Confirm Delete Task
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(); // If task doesn't exist, return 404
            }
            return View(task); // Show delete confirmation page
        }

        // POST: Delete the Task
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Read"); // Redirect to task list after deletion
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = true;
            task.Status = "Completed";

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Read"); // Redirect to Task List
        }
    }
}
