using AutoMapper;
using Data.FormModels;
using EMS.Constants;
using EMS.Data;
using EMS.Data.Entities.Emp;
using EMS.Data.FormModels.EmpSalary;
using EMS.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System.Drawing.Printing;
using Syncfusion.Drawing;
using System.Data;

namespace Ems1.Controllers
{
    public class AmountController : Controller
    {
        private readonly IAmountService _amountService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<Employee> _userManager;
        private EmpDbContext _applicationDbContext;

        public AmountController(IAmountService amountService, IEmailSender emailSender, IMapper mapper, UserManager<Employee> userManager, EmpDbContext applicationDbContext)
        {
            _amountService = amountService;
            _emailSender = emailSender;
            _mapper = mapper;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [Authorize(Roles = "Superadmin")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employee);
            var list = users.Select(x => new SelectListItem { Text = x.UserName, Value = x.Id }).ToList();
            ViewBag.Employee = list;
            return View();
        }

        // POST: Amount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmpSalaryVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string employeeId = _userManager.GetUserId(User);
                var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employee);
                SelectList list = new SelectList(users);
                ViewBag.Employee = list;

                if (model is not null && !string.IsNullOrEmpty(model.RequestingEmpId))
                {
                    if (_amountService.IsSalaryForEmployeeExist(model.RequestingEmpId, DateTime.Now.Month))
                    {
                        ModelState.AddModelError("", "Employee Salary already Credited...");
                        return View(model);
                    }
                    else
                    {
                        var emp = await _userManager.FindByIdAsync(model.RequestingEmpId);
                        EmpSalaryVM amountreqmodel = new EmpSalaryVM
                        {
                            RequestingEmpId = model.RequestingEmpId,
                            SalaryAmount = model.SalaryAmount,
                            Designation = model.Designation,
                            Department = model.Department,
                            GrossSalary = model.GrossSalary,
                            TotalWorkingDays = model.TotalWorkingDays,
                            PaidDays = model.PaidDays,
                            LOPDays = model.LOPDays,
                            BankName = model.BankName,
                            AccountNumber = model.AccountNumber,
                            HRA = model.HRA,
                            NetSalary = model.NetSalary,
                            ConveyanceAllowance = model.ConveyanceAllowance,
                            MedicalAllowance = model.MedicalAllowance,
                            EPF = model.EPF,
                            ProfessionalTax = model.ProfessionalTax,
                            LeaveDeduction = model.LeaveDeduction,
                            DailySalary = model.DailySalary,
                            TotalLeaves = model.TotalLeaves,
                            Salarydate = DateTime.Now,
                            CreatedbyId = employeeId
                        };

                        EmpSalary amountRequest = _mapper.Map<EmpSalary>(amountreqmodel);
                        amountRequest.CreatedBy = _userManager.GetUserName(User);
                        await _amountService.AddEmployeeSalary(amountRequest);

                        // Send Email to supervisor and requesting user
                        await _emailSender.SendEmailAsync("admin@localhost.com", "New Leave Request",
                            $"Please review this leave request. <a href='UrlOfApp/{amountRequest.Id}'>Click Here</a>");

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong...");
                    return View(model);
                }
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Something went wrong", ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<ActionResult> EmpIndex()
        {
            string employeeid = _userManager.GetUserId(User);      
            var employeeRequests = await _amountService.GetSalaryForEmployee(employeeid);
            foreach (var empSalary in employeeRequests)
            {
                var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employee);
                empSalary.RequestingEmployee = users.Where(x => x.Id == empSalary.RequestingEmpId).FirstOrDefault();
            }

            List<EmpSalaryVM> employeeRequestsModel = _mapper.Map<List<EmpSalaryVM>>(employeeRequests);
            EmployeeDataVm model = new EmployeeDataVm
            {
                AmountVMs = employeeRequestsModel,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPayslip(int empSalaryid)
        {
            // Retrieve data from your database or other sources to fill the PDF content
            string employeeid = _userManager.GetUserId(User);
            var employeeRequests = await _amountService.GetSalaryForEmployee(employeeid);
            EmpSalary empSalarydata = new EmpSalary();

            if(employeeRequests is not null && employeeRequests.Any(x => x.Id == empSalaryid))
            {
                empSalarydata = employeeRequests.FirstOrDefault(x => x.Id == empSalaryid);
            }

            string filename = $"Emp_{ employeeid} _payslip";
            //
            //Creates a new PDF document.
            PdfDocument document = new PdfDocument();
            //Adds page settings.
            document.PageSettings.Orientation = PdfPageOrientation.Landscape;
            document.PageSettings.Margins.All = 50;
            //Adds a page to the document.
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/MDTIcon.png");
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image file not found");
            }

            FileStream imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            RectangleF bounds = new RectangleF(176, 0, 390, 130);
            PdfImage image = PdfImage.FromStream(imageStream);
            //Draws the image to the PDF page.
            page.Graphics.DrawImage(image, bounds);

            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page.
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number.
            PdfTextElement element = new PdfTextElement("INVOICE " + empSalaryid.ToString(), subHeadingFont);
            element.Brush = PdfBrushes.White;

            //Draws the heading on the page.
            PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));
            string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            //Measures the width of the text to place it in the correct location.
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);
            //Draws the date by using DrawString method.
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);
            PdfFont timesRoman = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
            //Creates text elements to add the address and draw it to the page.
            element = new PdfTextElement("BILL TO ", timesRoman);
            element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            //Draws a line at the bottom of the address.
            graphics.DrawLine(linePen, startPoint, endPoint);
            //

            //Creates the datasource for the table.
            DataTable invoiceDetails = new DataTable();
            //Creates a PDF grid.
            PdfGrid grid = new PdfGrid();
            //Adds the data source.
            grid.DataSource = invoiceDetails;
            //Creates the grid cell styles.
            PdfGridCellStyle cellStyle = new PdfGridCellStyle();
            cellStyle.Borders.All = PdfPens.White;
            PdfGridRow header = grid.Headers[0];
            //Creates the header style.
            PdfGridCellStyle headerStyle = new PdfGridCellStyle();
            headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
            headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            headerStyle.TextBrush = PdfBrushes.White;
            headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

            //Adds cell customizations.
            for (int i = 0; i < header.Cells.Count; i++)
            {
                if (i == 0 || i == 1)
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                else
                    header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            }

            //Applies the header style.
            header.ApplyStyle(headerStyle);
            cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
            cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
            cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
            //Creates the layout format for grid.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            //Creates layout format settings to allow the table pagination.
            layoutFormat.Layout = PdfLayoutType.Paginate;
            //Draws the grid to the PDF page.
            PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

            //Create file stream. 
            FileStream fileStream = new FileStream("Sample.pdf", FileMode.Create, FileAccess.ReadWrite);
            //Save and close the PDF document 
            document.Save(fileStream);
            document.Close(true);

            return File(fileStream, "application/pdf");
        }

    [Authorize(Roles = "Superadmin")]
        public async Task<ActionResult> Index()
        {
            List<EmpSalary> empSalaries = _amountService.FindAll();
            foreach (var empSalary in empSalaries)
            {
                if(empSalary.RequestingEmployee is null && !string.IsNullOrEmpty(empSalary.RequestingEmpId))
                {
                    var users = await _userManager.GetUsersInRoleAsync(UserRoles.Employee);
                    empSalary.RequestingEmployee = users.Where(x => x.Id == empSalary.RequestingEmpId).FirstOrDefault();
                }
            }
            List<EmpSalaryVM> empSalaryVM = _mapper.Map<List<EmpSalaryVM>>(empSalaries);
            AmountRequestVM model = new AmountRequestVM
            {
                Amounts = empSalaryVM
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                EmpSalary empSalary = await _amountService.GetEmpSalary(id);
                if (empSalary == null)
                {
                    return NotFound();
                }

                await _amountService.RemoveEmpSalary(empSalary.Id);
            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }

    }
}

