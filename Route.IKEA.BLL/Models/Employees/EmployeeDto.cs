using Route.IKEA.DAL.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.IKEA.BLL.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        //[Required]
        //[MaxLength(50,ErrorMessage ="Max length of name is 50 char")]
        //[MinLength(2, ErrorMessage = "Min length of name is 2 char")]



        //   [Range(22, 30, ErrorMessage = "Age must be between 22 and 30")]
        public int? Age { get; set; }

        //  [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
        //  ErrorMessage = "Address must be in the format '123-Street-City-Country'")]
        public string? Address { get; set; }

        //[DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        //  [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        //  [EmailAddress]
        public string? Email { get; set; }


        public string Gender { get; set; } = null!;

        public string EmployeeType { get; set; }=null!;
    }
}
