using Microsoft.AspNetCore.Http;
using Route.IKEA.BLL.Validations.Enums;
using Route.IKEA.DAL.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Models
{
    public class CreateEmployeeDto
    {
        [MaxLength(50, ErrorMessage = "Max Length is 50 Chars")]
        [MinLength(5, ErrorMessage = "Min Length is 5 Chars")]
        public string Name { get; set; } = null!;
        [Range(22, 30)]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
                            , ErrorMessage = "Address Must Be Like 123-Street-City-Country")]
        public string? Address { get; set; }
        public decimal Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Hiring Date")]
        public DateOnly HiringDate { get; set; }
        [EnumNotDefault(ErrorMessage = "Please select Gender")]
        public Gender Gender { get; set; }
        [Display(Name = "Employee Type")]
        [EnumNotDefault(ErrorMessage = "Please select EmployeeType")]
        public EmployeeType EmployeeType { get; set; }
        public DateTime CreationDate { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
