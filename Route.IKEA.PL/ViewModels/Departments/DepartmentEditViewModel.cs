using System.ComponentModel.DataAnnotations;

namespace Route.IKEA.PL.ViewModels.Departments
{
    public class DepartmentEditViewModel
    {
        [Required(ErrorMessage ="Code is Required Ya Habiby") ]
        public string Code { get; set; } = null!;
        public String Name { get; set; } = null!;
        public String? Description { get; set; }

        [Display(Name = "Creation Date")]
        public DateOnly CreationDate { get; set; }


    }
}
