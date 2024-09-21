using Microsoft.AspNetCore.Mvc;
using Route.IKEA.BLL.Models;
using Route.IKEA.BLL.Services.Employees;
using Route.IKEA.PL.ViewModels.Employees;
using Microsoft.Extensions.Logging;

namespace Route.IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {
        #region Services
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IHostEnvironment _environment;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger, IHostEnvironment environment)
        {
            _employeeService = employeeService;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var employees = string.IsNullOrWhiteSpace(searchTerm) ? _employeeService.GetAllEmployee() : _employeeService.SearchEmployeeByName(searchTerm);

            return View(employees);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            var message = string.Empty;

            try
            {
                var result = _employeeService.CreateEmployee(employee);

                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    message = "Employee is not created.";

                ModelState.AddModelError(string.Empty, message);
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "An error occurred during employee creation.";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(employee);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee == null)
                return NotFound();

            return View(employee);
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = _employeeService.GetEmployeeById(id.Value);

            if (employee is null)
                return NotFound();

            return View(new EmployeeEditViewModel()
            {
                Name = employee.Name,
                Age = employee.Age,
                Address = employee.Address,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                Email = employee.Email,
                Gender = employee.Gender,
                EmployeeType = employee.EmployeeType
            });
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, EmployeeEditViewModel employeeVM)
        {
            if (!ModelState.IsValid)
                return View(employeeVM);

            var message = string.Empty;
            try
            {
                var employeeToUpdate = new UpdatedEmployeeDto()
                {
                    Id = id,
                    Name = employeeVM.Name,
                    Age = employeeVM.Age,
                    Address = employeeVM.Address,
                    Salary = employeeVM.Salary,
                    IsActive = employeeVM.IsActive,
                    Email = employeeVM.Email,
                   
                };

                var updated = _employeeService.UpdateEmployee(employeeToUpdate) > 0;
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
            return View(employeeVM);
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = _employeeService.DeleteEmployee(id);

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
