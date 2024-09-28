using AutoMapper;
using Route.IKEA.BLL.Models;
using Route.IKEA.BLL.Models.Departments;
using Route.IKEA.PL.ViewModels.Departments;
using Route.IKEA.PL.ViewModels.Employees;

namespace Route.IKEA.PL.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            #region Employee
            CreateMap<UpdatedEmployeeDto, EmployeeEditViewModel>();
            CreateMap<EmployeeEditViewModel, UpdatedEmployeeDto>();
            CreateMap<EmployeeDetailsDto, UpdatedEmployeeDto>(); // لتحويل الـ EmployeeDetailsDto إلى UpdatedEmployeeDto
            CreateMap<CreateEmployeeDto, EmployeeEditViewModel>();
            CreateMap<EmployeeEditViewModel, CreateEmployeeDto>();
            #endregion

            #region Department
            CreateMap<DepartmentDetailsDto, DepartmentEditViewModel>();
            CreateMap<DepartmentEditViewModel,UpdatedDepartmentDto>();
            CreateMap<DepartmentEditViewModel, CreateDepartmentDto>();



            #endregion
        }
    }
}
