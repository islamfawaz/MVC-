﻿using Microsoft.EntityFrameworkCore;
using Route.IKEA.DAL.Entities.Department;
using Route.IKEA.DAL.Presistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.DAL.Presistence.Repositories.Departments
{
    public class DepartmentRepository : IDepartmentRepository
    {

        private ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Department> GetAll(bool withAsNoTracking = true )
        {
            if (withAsNoTracking)
                return _dbContext.Departments.AsNoTracking().ToList();
            
            else
                return _dbContext.Departments.AsNoTracking().ToList();

        }

        public Department? GetById(int id)
        {
            return _dbContext.Departments.Find(id);
        }

        public int Add(Department entity)
        {
            _dbContext.Departments.Add(entity);
            return _dbContext.SaveChanges();
        }



        public int Update(Department entity)
        {
            _dbContext.Departments.Update(entity);
            return _dbContext.SaveChanges();    
        }


        public int Delete(Department entity)
        {
            _dbContext.Departments.Remove(entity);
            return _dbContext.SaveChanges();
        }



       
    }
}
