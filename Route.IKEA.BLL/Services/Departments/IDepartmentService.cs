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
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();

        Task<DepartmentDetailsDto?>  GetDepartmentByIdAsync(int id); 
        Task<int> CreateDepartmentAsync(CreateDepartmentDto entity);

        Task<int> UpdateDepartmentAsync(UpdatedDepartmentDto entity);

        Task<bool> DeleteDepartmentAsync(int entity);

        IEnumerable<DepartmentDto> SearchDepartments(string name);

    }
}
