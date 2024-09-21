﻿using Route.IKEA.BLL.Models;
using Route.IKEA.DAL.Entities.Employees;
using Route.IKEA.DAL.Presistence.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Route.IKEA.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public int CreateEmployee(CreateEmployeeDto entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var employee = new Employee
            {
                Name = entity.Name,
                Age = entity.Age,
                Address = entity.Address,
                Salary = entity.Salary,
                IsActive = entity.IsActive,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                HiringDate = entity.HiringDate,
                Gender = entity.Gender,
                EmployeeType = entity.EmployeeType
            };

            return _employeeRepository.Add(employee);
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null)
                return false;

            _employeeRepository.Delete(employee);
            return true;
        }

        public IEnumerable<EmployeeDto> GetAllEmployee()
        {
            return _employeeRepository.GetAll()
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Age = e.Age,
                    Address = e.Address,
                    Salary = e.Salary,
                    IsActive = e.IsActive,
                    Email = e.Email,
                    Gender = e.Gender.ToString(),
                    EmployeeType = e.EmployeeType.ToString()
                });
        }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee == null)
                return null;

            return new EmployeeDetailsDto
            {
                Id = employee.Id,
                Age = employee.Age,
                Address = employee.Address,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                Email = employee.Email,
                Gender = employee.Gender.ToString(),
                EmployeeType = employee.EmployeeType.ToString()
            };
        }

        public IEnumerable<EmployeeDto> SearchEmployeeByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<EmployeeDto>();

            var employees = _employeeRepository.SearchByName(name);

            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Age = e.Age,
                Address = e.Address,
                Salary = e.Salary,
                IsActive = e.IsActive,
                Email = e.Email,
                Gender = e.Gender.ToString(),
                EmployeeType = e.EmployeeType.ToString()
            });
        }

        public int UpdateEmployee(UpdatedEmployeeDto entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var employee = _employeeRepository.GetById(entity.Id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            employee.Name = entity.Name;
            employee.Age = entity.Age;
            employee.Address = entity.Address;
            employee.Salary = entity.Salary;
            employee.IsActive = entity.IsActive;
            employee.Email = entity.Email;
            employee.PhoneNumber = entity.PhoneNumber;
            employee.HiringDate = entity.HiringDate;
            employee.Gender = entity.Gender;
            employee.EmployeeType = entity.EmployeeType;

            return _employeeRepository.Update(employee);
        }
    }
}