using Route.IKEA.BLL.Models.Departments;
using Route.IKEA.DAL.Entities.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Services.Departments
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto>GetAllDepartments();

        DepartmentDetailsDto ? GetDepartmentById(int id); 
        int CreateDepartment(CreateDepartmentDto entity);

        int UpdateDepartment(UpdatedDepartmentDto entity);

        bool DeleteDepartment(int entity);

        IEnumerable<DepartmentDto> SearchDepartments(string name);

    }
}
