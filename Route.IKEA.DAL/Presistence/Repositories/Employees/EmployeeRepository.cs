using Route.IKEA.DAL.Entities.Employees;
using Route.IKEA.DAL.Presistence.Data;
using Route.IKEA.DAL.Presistence.Repositories._Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Presistence.Repositories.Employees
{
    public class EmployeeRepository :GenericRepository<Employee> ,IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext) :base(dbContext)
        {
            
        }
    }
}
