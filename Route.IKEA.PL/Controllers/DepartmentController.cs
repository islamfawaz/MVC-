using AutoMapper;
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
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IHostEnvironment environment, IMapper mapper)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _mapper = mapper;

        }
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var employees = string.IsNullOrWhiteSpace(searchTerm) ? _departmentService.GetAllDepartments() : _departmentService.SearchDepartments(searchTerm);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Partials/DepartmentTablePartialView", employees);
            }

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
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);

            var message = string.Empty;

            try
            {

              
                var CreatedDepartment=_mapper.Map<CreateDepartmentDto>(departmentVM);

                var created = _departmentService.CreateDepartment(CreatedDepartment) > 0;

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
            return View(departmentVM);
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

            var departmentVM = _mapper.Map<DepartmentDetailsDto,DepartmentEditViewModel>(department);
            return View(departmentVM);
           
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
                //var DepartmentToUpdate = new UpdatedDepartmentDto()
                //{
                //    Id = id,
                //    Name = departmentVM.Name,
                //    Code = departmentVM.Code,
                //    CreationDate = departmentVM.CreationDate,
                //    Description = departmentVM.Description,
                //};
                var UpdatedDepartment = _mapper.Map<UpdatedDepartmentDto>(departmentVM);

                var Updated = _departmentService.UpdateDepartment(UpdatedDepartment) > 0;
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
