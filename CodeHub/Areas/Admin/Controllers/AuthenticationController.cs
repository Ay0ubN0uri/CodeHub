using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CodeHub.Models.ViewModels;
using CodeHub.Models.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace CodeHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthenticationController : Controller
    {

		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly ILogger<AuthenticationController> _logger;

		public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthenticationController> logger)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
		}


		/*public IActionResult Login()
	    {
		    return View();
	    }*/


		[HttpGet]
		public async Task<IActionResult> Login(string returnUrl = null)
		{
			// Check if the user is already authenticated
			if (User.Identity.IsAuthenticated)
			{
				var user = await _userManager.GetUserAsync(User);
				var isAdmin  = await _userManager.IsInRoleAsync(user, "Admin");
				if (isAdmin)
				{
					// Redirect to the Admin dashboard
					return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
				}
				else
				{
					// Redirect to the Home page for normal users
					return RedirectToAction("Index", "Home", new { Area = "Customer" });
				}
				// Redirect to the dashboard or home page if the user is already logged in
				//return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
			}

			// If not authenticated, proceed to show the login view
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(email);
				if(user != null)
				{
					var result = await _signInManager.PasswordSignInAsync(user.UserName, password, rememberMe, lockoutOnFailure: false);
					if (result.Succeeded)
					{
						var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
						if (isAdmin)
						{
							// Redirect to the Admin dashboard
							return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
						}
						else
						{
							// Redirect to the Home page for normal users
							return RedirectToAction("Index", "Home", new { Area = "Customer" });
						}
						/*return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });*/
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Invalid login attempt.");
						_logger.LogError("Invalid login attempt.");
						return View();
					}
				}
				else
				{
					_logger.LogError("User not found!");
				}
			}else
			{
				foreach (var modelStateKey in ViewData.ModelState.Keys)
				{
					var modelStateVal = ViewData.ModelState[modelStateKey];
					foreach (var error in modelStateVal.Errors)
					{
						var key = modelStateKey;
						var errorMessage = error.ErrorMessage;
						_logger.LogError($"Error in {key}: {errorMessage}");
					}
				}
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");
			return RedirectToAction(nameof(Login));
		}

		public IActionResult Register()
	    {
            return View();
	    }


		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new User{ 
					UserName = model.UserName, 
					FirstName = model.FirstName,  
					LastName = model.LastName,
					Email = model.Email 
				};

				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					// Add user to the User role
					await _userManager.AddToRoleAsync(user, "User");

					// Optionally sign the user in after registration
					await _signInManager.SignInAsync(user, isPersistent: false);

					return RedirectToAction("Index", "Home", new { Area = "Customer" });
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			else
			{
				foreach (var modelStateKey in ViewData.ModelState.Keys)
				{
					var modelStateVal = ViewData.ModelState[modelStateKey];
					foreach (var error in modelStateVal.Errors)
					{
						var key = modelStateKey;
						var errorMessage = error.ErrorMessage;
						_logger.LogError($"Error in {key}: {errorMessage}");
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//public IActionResult UnlockUser()
		//{
		//    return View();
		//}
		//public IActionResult ForgetPassword()
		//{
		//    return View();
		//}
		//public IActionResult ResetPassword()
		//{
		//    return View();
		//}
		//public IActionResult Maintenance()
		//{
		//    return View();
		//}
	}
}
