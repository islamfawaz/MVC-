using System.ComponentModel.DataAnnotations;

namespace Route.IKEA.PL.ViewModels.Identity
{
	public class ForgetPaswwordViewModel
	{
		[EmailAddress]
		public string Email { get; set; } = null!;
	}
}
