using Route.IKEA.DAL.Presistence.Repositories.Departments;
using Route.IKEA.DAL.Presistence.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Presistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository DepartmentRepository { get;  }
        IEmployeeRepository EmployeeRepository { get; }


        int Complete();
    }
}
