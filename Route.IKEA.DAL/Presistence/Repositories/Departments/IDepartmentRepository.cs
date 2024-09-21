using Route.IKEA.DAL.Entities.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Presistence.Repositories.Departments
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll(bool withAsNoTracking=true);
        IQueryable<Department> GetAllAsIQueryable();

        Department? GetById(int id);

        int Add(Department entity);

        int Update(Department entity);

        int Delete(Department entity);

        IEnumerable<Department> SearchDepartmentsByName(string name);

    }
}
