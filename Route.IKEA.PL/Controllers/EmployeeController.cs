using LinkDev.IKEA.BLL.Common.Services.Attachments;
using Route.IKEA.BLL.Models;
using Route.IKEA.BLL.Services.Employees;
using Route.IKEA.PL.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Route.IKEA.BLL.Services.Departments;
using Microsoft.AspNetCore.Authorization;

namespace Route.IKEA.PL.Controllers
{
	[Authorize]

	public class EmployeeController : Controller
    {
        #region Services
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IHostEnvironment _environment;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger,
                                  IHostEnvironment environment, IMapper mapper)
        {
            _employeeService = employeeService;
            _logger = logger;
            _environment = environment;
            _mapper = mapper;
        }
        #endregion

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var employees = string.IsNullOrWhiteSpace(searchTerm)
                ?  _employeeService.GetAllEmployeeAsync()
                : await Task.Run(() => _employeeService.SearchEmployeeByName(searchTerm));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("PartialViews/EmployeeTablePartialView", employees);
            }

            return View(employees);
        }

        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create([FromServices] IDepartmentService departmentService)
        {
            ViewData["Departments"] = await departmentService.GetAllDepartmentsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            var message = string.Empty;

            try
            {
                var result = await _employeeService.CreateEmployeeAsync(employee);

                if (result > 0)
                    TempData["message"] = "Employee is created";
                else
                    TempData["message"] = "Employee is not created";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "An error occurred during employee creation.";
            }

            ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
                return NotFound();

            return View(employee);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, [FromServices] IDepartmentService departmentService)
        {
            if (id is null)
                return BadRequest();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
                return NotFound();

            ViewData["Departments"] = await departmentService.GetAllDepartmentsAsync();

            var employeeDto = _mapper.Map<UpdatedEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            var message = string.Empty;
            try
            {
                var employeeVM = _mapper.Map<UpdatedEmployeeDto>(employee);
                var updated = await _employeeService.UpdateEmployeeAsync(employeeVM) > 0;

                if (updated)
                    return RedirectToAction(nameof(Index));
                message = "An error occurred during employee update.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "An error occurred during employee update.";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(employee);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = await _employeeService.DeleteEmployeeAsync(id);

                if (deleted)
                    return RedirectToAction(nameof(Index));
                else
                    message = "An error occurred during employee deletion.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "An error occurred during employee deletion.";
            }

            ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
