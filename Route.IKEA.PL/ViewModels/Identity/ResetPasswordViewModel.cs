using System.ComponentModel.DataAnnotations;

namespace Route.IKEA.PL.ViewModels.Identity
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
		[DataType(DataType.Password)]

		[Compare("Password", ErrorMessage = "Confrim Password doesn't match with Paswword")]
		public string ConfirmPassword { get; set; } = null!;

	}
}
