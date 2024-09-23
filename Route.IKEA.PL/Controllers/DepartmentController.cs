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

        #region Index
        [HttpGet]

        public IActionResult Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            // View's Dictionary : Pass Data from Controller [Action] to view (from view)
            //
            // 1. ViewData is a dictionary type property (introduced in asp.net Framework 3.0
            // => it helps us to transfer the data from controller [action] to view 

            //ViewData["Message"] = "Hello VieData";

            // 2. ViewBag is a dynamic type property ( introduced in asp.net framework 4.0
            //  => it helps us to transfer the data from controller [Action] to View

            // View.Message = "Hello ViewBag";
            // View.Message = new {Id= 10 , Name = "Ali"};



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
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentEditViewModel department)
        {
            if (!ModelState.IsValid)
                return View(department);

            var message = string.Empty;

            try
            {

                var createdDepartment = new CreateDepartmentDto()
                {
                    Name = department.Name,
                    Code = department.Code,
                    CreationDate = department.CreationDate,
                    Description = department.Description,
                };

                var created = _departmentService.CreateDepartment(createdDepartment) > 0;

                // 3. TempData is a property of type Dictionary object (Introduced in asp.net framework 3.5)
                //     => used to transfer data between two active requests 


                if (created)
                TempData["message"] = "Department Is created";

                else
                   TempData["message"] = "Department Is Not created";
                    //message = "Department is not Created";

              

                ModelState.AddModelError(string.Empty, message);
                    return RedirectToAction(nameof(Index));


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
        [ValidateAntiForgeryToken]

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
        [ValidateAntiForgeryToken]

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
