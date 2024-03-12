using CodeHub.Models.Models;
using CodeHub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeHub.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize(Roles = "Admin, User")]
	public class ProfileController : Controller
	{
		private readonly UserManager<User> _userManager;

		public ProfileController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			var viewModel = new UserInfoViewModel
			{
				Username = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email
			};
			return View(viewModel);
		}
	}
}
