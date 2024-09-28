﻿using Microsoft.EntityFrameworkCore;
using Route.IKEA.BLL.Models.Departments;
using Route.IKEA.DAL.Entities.Department;
using Route.IKEA.DAL.Presistence.Repositories.Departments;
using Route.IKEA.DAL.Presistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public int CreateDepartment(CreateDepartmentDto departmentDto)
        {
            var department = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                //  CreatedOn = DateTime.UtcNow,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,

            };

             _unitOfWork.DepartmentRepository.Add(department);
            return _unitOfWork.Complete();
        }

        public bool DeleteDepartment(int id)
        {
            var DepartmentRepo =_unitOfWork.DepartmentRepository;
            var department = DepartmentRepo.GetById(id);
            if (department is { })
                 DepartmentRepo.Delete(department);

            return _unitOfWork.Complete()>0;

        }

        public IEnumerable<DepartmentDto> GetAllDepartments()
        {
            var department = _unitOfWork.DepartmentRepository.GetAllAsIQueryable()
                .Where(d => d.IsDeleted != true)
                .Select(department => new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                CreationDate = department.CreationDate,
            }).AsNoTracking().ToList();
            return department;
        }

        public DepartmentDetailsDto? GetDepartmentById(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
            if (department is not null)
                return new DepartmentDetailsDto()
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    Description = department.Description,
                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModifiedBy = department.LastModifiedBy,
                    LastModifiedOn = department.LastModifiedOn,


                };
            return null;
        }

        public int UpdateDepartment(UpdatedDepartmentDto departmentDto)
        {
            var department = new Department()
            {
                Id = departmentDto.Id,
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,

            };

            _unitOfWork.DepartmentRepository.Update(department);
            return _unitOfWork.Complete();
        }

        public IEnumerable<DepartmentDto> SearchDepartments(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Search term cannot be empty or null.", nameof(name));
            }

            var departments = _unitOfWork.DepartmentRepository.SearchByName(name);

            var Searched = departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
             , CreationDate = d.CreationDate,
                Code = d.Code,
                    
            });

            return Searched;
        }

      
    }
}
