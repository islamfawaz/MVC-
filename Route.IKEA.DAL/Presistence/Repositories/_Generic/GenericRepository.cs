using Microsoft.EntityFrameworkCore;
using Route.IKEA.DAL.Entities;
using Route.IKEA.DAL.Entities.Department;
using Route.IKEA.DAL.Presistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Route.IKEA.DAL.Presistence.Repositories._Generic
{
    public class GenericRepository<T> :IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<T> GetAll(bool withAsNoTracking = true)
        {
            return withAsNoTracking
                ? _dbContext.Set<T>().Where(X => X.IsDeleted != true).AsNoTracking().ToList()
                : _dbContext.Set<T>().ToList();
        }

        public T? GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public void Add(T entity)
        =>  _dbContext.Set<T>().Add(entity);
        

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Update(entity);
        }

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);
        

        public IQueryable<T> GetAllAsIQueryable()
        {
            return _dbContext.Set<T>();
        }
        public IEnumerable<T> GetAllAsIEnumerable()
        {
            return _dbContext.Set<T>();
        }
        public IEnumerable<T> SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<T>();
            }

            return _dbContext.Set<T>()
                .Where(d => d.Name != null && d.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        
    }
}
