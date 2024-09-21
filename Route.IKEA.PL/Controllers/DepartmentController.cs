using Microsoft.AspNetCore.Mvc;
using Route.IKEA.BLL.Models;
using Route.IKEA.BLL.Models.Departments;
using Route.IKEA.BLL.Services.Departments;
using Route.IKEA.DAL.Entities.Department;
using Route.IKEA.PL.ViewModels.Departments;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Route.IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IHostEnvironment _environment;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IHostEnvironment environment)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region MyRegion
        [HttpGet]

        public IActionResult Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var departments = string.IsNullOrWhiteSpace(searchTerm) ? _departmentService.GetAllDepartments() : _departmentService.SearchDepartments(searchTerm);

            return View(departments);
        }
        #endregion

        #region Create
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

                message = _environment.IsDevelopment() ? ex.Message : "an errror has occured during updating the department: (";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(department);
        }


        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = _departmentService.GetDepartmentById(id.Value);
            if (department == null)
                return NotFound();
            return View(department);
        }
        #endregion

        #region Edit
        [HttpGet]//GET /Department/Edit/Id
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);

            if (department is null)
                return NotFound();

            return View(new DepartmentEditViewModel()
            {
                Name = department.Name,
                Code = department.Code,
                CreationDate = department.CreationDate,
                Description = department.Description,
            });
        }

        [HttpPost]//

        public IActionResult Edit([FromRoute] int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var DepartmentToUpdate = new UpdatedDepartmentDto()
                {
                    Id = id,
                    Name = departmentVM.Name,
                    Code = departmentVM.Code,
                    CreationDate = departmentVM.CreationDate,
                    Description = departmentVM.Description,
                };

                var Updated = _departmentService.UpdateDepartment(DepartmentToUpdate) > 0;
                if (Updated)
                    return RedirectToAction(nameof(Index));
                message = "an errror has occured during updating the department :(";

            }
            catch (Exception ex)
            {
                //log exception 
                _logger.LogError(ex, ex.Message);
                //set message
                message = _environment.IsDevelopment() ? ex.Message : "an errror has occured during updating the department: (";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);

        }

        #endregion

        #region Delete
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound();

            return View(department);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var Deleted = _departmentService.DeleteDepartment(id);

                if (Deleted)
                    return RedirectToAction(nameof(Index));
                else
                    message = "an error ocurr during delete department :(";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                message = _environment.IsDevelopment() ? ex.Message : "\"an error ocurr during delete department :(";

            }
            ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Index));



        } 
        #endregion

    }
}
