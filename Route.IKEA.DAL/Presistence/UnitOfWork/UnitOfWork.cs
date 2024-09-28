using Route.IKEA.DAL.Presistence.Data;
using Route.IKEA.DAL.Presistence.Repositories.Departments;
using Route.IKEA.DAL.Presistence.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Presistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
          
        }

        public IDepartmentRepository DepartmentRepository {
            get
            {  
                return new DepartmentRepository(_dbContext);
            }

        }
        public IEmployeeRepository EmployeeRepository { 
            get
            {
                return new EmployeeRepository(_dbContext);
            }
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }
        public void Dispose() { 
            _dbContext.Dispose();
        }
    }
}
