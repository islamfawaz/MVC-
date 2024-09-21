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
        IEnumerable<EmployeeDto> GetAllEmployee();

        EmployeeDetailsDto? GetEmployeeById(int id);
        int CreateEmployee(CreateEmployeeDto entity);

        int UpdateEmployee(UpdatedEmployeeDto entity);

        bool DeleteEmployee(int entity);

        IEnumerable<EmployeeDto> SearchEmployeeByName(string name);

    }
}
