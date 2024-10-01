using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Route.IKEA.BLL.Models;
using Route.IKEA.BLL.Models.Departments;
using Route.IKEA.BLL.Services.Departments;
using Route.IKEA.PL.ViewModels.Departments;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Route.IKEA.PL.Controllers
{
	[Authorize]

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
        public async Task<IActionResult> Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var departments = string.IsNullOrWhiteSpace(searchTerm)
                ? await _departmentService.GetAllDepartmentsAsync()
                : _departmentService.SearchDepartments(searchTerm);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("Partials/DepartmentTablePartialView", departments);
            }

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
        public async Task<IActionResult> Create(DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);

            var message = string.Empty;

            try
            {
                var createdDepartment = _mapper.Map<CreateDepartmentDto>(departmentVM);

                var created = await _departmentService.CreateDepartmentAsync(createdDepartment) > 0;

                if (created)
                    TempData["message"] = "Department is created.";
                else
                    TempData["message"] = "Department is not created.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "An error occurred during department creation.";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department == null)
                return NotFound();

            return View(department);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
                return NotFound();

            var departmentVM = _mapper.Map<DepartmentEditViewModel>(department);
            return View(departmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);

            var message = string.Empty;

            try
            {
                var updatedDepartment = _mapper.Map<UpdatedDepartmentDto>(departmentVM);

                var updated = await _departmentService.UpdateDepartmentAsync(updatedDepartment) > 0;
                if (updated)
                    return RedirectToAction(nameof(Index));

                message = "An error occurred during department update.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "An error occurred during department update.";
            }

            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;

            try
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);

                if (deleted)
                    return RedirectToAction(nameof(Index));

                message = "An error occurred during department deletion.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "An error occurred during department deletion.";
            }

            ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
