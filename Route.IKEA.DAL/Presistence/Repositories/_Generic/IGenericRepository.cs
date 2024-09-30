using Route.IKEA.DAL.Entities.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Presistence.Repositories._Generic
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync(bool withAsNoTracking = true);
        IQueryable<T> GetAllAsIQueryable();
        IEnumerable<T> GetAllAsIEnumerable();

        Task<T?> GetByIdAsync(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        IEnumerable<T> SearchByName(string name);

    }
}
