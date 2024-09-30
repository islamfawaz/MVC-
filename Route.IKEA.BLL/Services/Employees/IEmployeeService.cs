using Route.IKEA.BLL.Models;
using Route.IKEA.BLL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Services.Employees
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployeeAsync();

        Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(CreateEmployeeDto entity);

        Task<int> UpdateEmployeeAsync(UpdatedEmployeeDto entity);

        Task<bool> DeleteEmployeeAsync(int entity);

        IEnumerable<EmployeeDto> SearchEmployeeByName(string name);

    }
}
