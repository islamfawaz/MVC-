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

        public IActionResult Index()
        {
            return View();
        }
    }
}
