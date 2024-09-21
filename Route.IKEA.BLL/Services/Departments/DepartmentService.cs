using Microsoft.EntityFrameworkCore;
using Route.IKEA.BLL.Models;
using Route.IKEA.DAL.Entities.Department;
using Route.IKEA.DAL.Presistence.Repositories.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
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

            return _departmentRepository.Add(department);
        }

        public bool DeleteDepartment(int id)
        {
            var department = _departmentRepository.GetById(id);
            if (department is { })
                return _departmentRepository.Delete(department) > 0;

            return false;

        }

        public IEnumerable<DepartmentReturnDto> GetAllDepartments()
        {
            var department = _departmentRepository.GetAllAsIQueryable().Select(department => new DepartmentReturnDto
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
            var department = _departmentRepository.GetById(id);
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

            return _departmentRepository.Update(department);
        }

        public IEnumerable<DepartmentReturnDto> SearchDepartments(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Search term cannot be empty or null.", nameof(name));
            }

            var departments = _departmentRepository.SearchDepartmentsByName(name);

            var Searched = departments.Select(d => new DepartmentReturnDto
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
