using System.ComponentModel.DataAnnotations;

namespace Route.IKEA.PL.ViewModels.Identity
{
	public class SignUpViewModel
	{
		public string UserName { get; set; } = null!;

		[EmailAddress]
		public string Email { get; set; } = null!;
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
		[DataType(DataType.Password)]

		[Compare("Password",ErrorMessage ="Confrim Password doesn't match with Paswword")]
		public string ConfirmPassword { get; set; } = null!;

		[Display(Name ="First Name")]
        public string FName { get; set; } = null!;
		[Display(Name = "Last Name")]

		public string LName { get; set; } = null!;



		public bool IsAgree { get; set; }
    }
}
