using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.IKEA.DAL.Entities.Identity;
using Route.IKEA.PL.ViewModels.Identity;

namespace Route.IKEA.PL.Controllers
{
	public class AccountController : Controller
	{
		#region Services
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_signInManager = signInManager;
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
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = await _userManager.FindByNameAsync(model.Email);
			if (user != null)
			{
				var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, model.RememberMe);

				if (result.IsNotAllowed)
					ModelState.AddModelError(string.Empty, "Your account is not confirmed.");

				if (result.IsLockedOut)
					ModelState.AddModelError(string.Empty, "Your account is locked.");

				if (result.Succeeded)
					return RedirectToAction(nameof(HomeController.Index), "Home");
			}

			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return View(model);
		}
        #endregion

        #region Sign Out
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn), "Account"); 
        }
        #endregion
    }
}
