using Microsoft.AspNetCore.Mvc;
using Route.IKEA.BLL.Models;
using Route.IKEA.BLL.Services.Departments;

namespace Route.IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IHostEnvironment _environment;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger ,IHostEnvironment environment)
        {
            _departmentService = departmentService;
           _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var departments = string.IsNullOrWhiteSpace(searchTerm) ? _departmentService.GetAllDepartments() : _departmentService.SearchDepartments(searchTerm);

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto department)
        {
            if (!ModelState.IsValid)
                return View(department);

            var message = string.Empty;

            try
            {

                var Result = _departmentService.CreateDepartment(department);

                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    message = "Department is not Created";
                ModelState.AddModelError(string.Empty, message);
                return View(department);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(department);
                }
                else
                {
                    message = "Department is not Created";
                    return View("Error", message);
                }

            }
        }
    }
}
