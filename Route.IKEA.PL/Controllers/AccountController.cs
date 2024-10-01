using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Route.IKEA.DAL.Entities.Identity;
using Route.IKEA.PL.ViewModels.Identity;

namespace Route.IKEA.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
		public AccountController(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager)
        {
			_signInManager = signInManager;
            _userManager = userManager;
		}

        #region Sign Up
        [HttpGet]
        public IActionResult SignUp() //Get //Account/SignUp
        {
            return View();
        }

		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			// Check if the model state is valid and return the view with errors if not
			if (!ModelState.IsValid)
				return View(model);

			// Check if the username is already taken
			var existingUser = await _userManager.FindByNameAsync(model.UserName.ToLowerInvariant());
			if (existingUser != null)
			{
				ModelState.AddModelError(nameof(SignUpViewModel.UserName), "This username is already taken.");
				return View(model);
			}

			// Check if the email is already registered
			var existingEmailUser = await _userManager.FindByEmailAsync(model.Email.ToLowerInvariant());
			if (existingEmailUser != null)
			{
				ModelState.AddModelError(nameof(SignUpViewModel.Email), "This email is already registered.");
				return View(model);
			}

			// Create a new ApplicationUser based on the provided details
			var user = new ApplicationUser
			{
				FName = model.FName,
				LName = model.LName,
				UserName = model.Email.ToLowerInvariant(),  // Normalize email for UserName
				Email = model.Email.ToLowerInvariant(),
				IsAgree = model.IsAgree
			};

			// Try to create the user and check if the creation was successful
			var result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				// Provide feedback to the user upon successful registration
				TempData["SuccessMessage"] = "Your account has been created successfully. Please log in.";
				return RedirectToAction(nameof(SignIn));
			}

			// Add errors to the model state for any issues during user creation
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			// Return the view with the model and validation errors
			return View(model);
		}
		#endregion


		#region Sign IN
		[HttpGet]		
		public IActionResult SignIn()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if(!ModelState.IsValid) 
                return BadRequest();
			var user=await _userManager.FindByNameAsync(model.Email);

			if (user is { })
			{
				var flag=await _userManager.CheckPasswordAsync(user,model.Password);
				if (flag)
				{
					var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, model.RememberMe);

					if (result.IsNotAllowed)
						ModelState.AddModelError(string.Empty, "Your account is not Confirmend");

					if(result.IsLockedOut)
						ModelState.AddModelError(string.Empty, "Your account is Lock");



					if (result.Succeeded)
						return RedirectToAction(nameof(HomeController.Index), "Home");


            

				}
			}

			ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");

			return View(model);
		}


        #endregion
    }
}
