using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using Route.IKEA.DAL.Entities.Identity;
using Route.IKEA.PL.Helper;
using Route.IKEA.PL.ViewModels.Identity;
using Email = Route.IKEA.PL.ViewModels.Identity.Email;

namespace Route.IKEA.PL.Controllers
{
	public class AccountController : Controller
	{
		#region Services
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailSettings _mailSettings;
        private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IMailSettings mailSettings)
		{
			_signInManager = signInManager;
            _mailSettings = mailSettings;
            _userManager = userManager;
		} 
		#endregion

		#region Sign Up
		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var existingUser = await _userManager.FindByNameAsync(model.UserName.ToLowerInvariant());
			if (existingUser != null)
			{
				ModelState.AddModelError(nameof(SignUpViewModel.UserName), "This username is already taken.");
				return View(model);
			}

			var existingEmailUser = await _userManager.FindByEmailAsync(model.Email.ToLowerInvariant());
			if (existingEmailUser != null)
			{
				ModelState.AddModelError(nameof(SignUpViewModel.Email), "This email is already registered.");
				return View(model);
			}

			var user = new ApplicationUser
			{
				FName = model.FName,
				LName = model.LName,
				UserName = model.Email.ToLowerInvariant(),
				Email = model.Email.ToLowerInvariant(),
				IsAgree = model.IsAgree
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				TempData["SuccessMessage"] = "Your account has been created successfully. Please log in.";
				return RedirectToAction(nameof(SignIn));
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(model);
		}
		#endregion

		#region Sign In
		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]


		public async Task<IActionResult> SignIn(SignInViewModel account)
		{
			if (!ModelState.IsValid)
				return View(account);

			var user = await _userManager.FindByEmailAsync(account.Email);

			if (user != null)
			{
				var PasswordCheck = await _userManager.CheckPasswordAsync(user, account.Password);

				if (PasswordCheck)
				{
					var result = await _signInManager.PasswordSignInAsync(user, account.Password, account.RememberMe, true);//Generate Token key  

					if (result.IsNotAllowed)
						ModelState.AddModelError(string.Empty, "Your Account Is Not Confirmed Yet");

					if (result.IsLockedOut)
						ModelState.AddModelError(string.Empty, "Your Account Is Blocked");

					if (result.Succeeded)
						return RedirectToAction(nameof(HomeController.Index), "Home");


				}
			}

			ModelState.AddModelError(string.Empty, "Invalid Login Attempt");


			return View(account);
		}

		#endregion

		#region Sign Out
		public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn), "Account"); 
        }
		#endregion

		#region Forget Password

		[HttpGet]
		public IActionResult ForgetPassword()
		{
			return View();
		}


		[HttpPost]
		public async  Task<IActionResult> ForgetPassword(ForgetPaswwordViewModel model)
		{
            if (ModelState.IsValid)
            {
			  var User=await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
					//Create Token
					var token=await _userManager.GeneratePasswordResetTokenAsync(User);


					//Create Reset Password URl
					var url= Url.Action(action: "ResetPassword", controller: "Account", new {email=model.Email ,token=token},Request.Scheme);

					////Create Email
					//var email = new Email()	
					//{
					//	To = model.Email,
					//	Subject = "Reset Subject",
					//	Body=url,

					//};
					//send email
					//EmailSettings.SendEmail(email);
					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Subject",
						Body = url
					};
					_mailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
                }
				ModelState.AddModelError(string.Empty, errorMessage: "Invalid Operation, Please Try Again !");



            }
			return View(model);
        }

		[HttpGet]
		public IActionResult CheckYourInbox()
		{
			return View();
		}

		[HttpGet]

		public IActionResult ResetPassword(string email,string token)
		{
			TempData["email"]=email;
			TempData["token"]=token;
			return View();
		}

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve email and token from TempData and log for debugging
                var email = TempData.Peek("email") as string;
                var token = TempData.Peek("token") as string;

                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError(string.Empty, "Email not found.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(token))
                {
                    ModelState.AddModelError(string.Empty, "Token not found.");
                    return View(model);
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                    if (result.Succeeded)
                    {
                        TempData.Remove("email");
                        TempData.Remove("token");
                        return RedirectToAction(nameof(SignIn));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found for the given email.");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Operation, Please Try Again!");
            return View(model);
        }

        #endregion
    }
}
