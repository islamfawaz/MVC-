using Microsoft.AspNetCore.Mvc;
using Route.IKEA.BLL.Services.Departments;

namespace Route.IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private  readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var departments = string.IsNullOrWhiteSpace(searchTerm) ? _departmentService.GetAllDepartments() : _departmentService.SearchDepartments(searchTerm);

            return View(departments);
        }

    }
}
