using CodeHub.DataAccess.Repository;
using CodeHub.DataAccess.Repository.IRepository;
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
	public class SettingsController(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager) : Controller
	{

		public async Task<IActionResult> Index()
		{
			var user = await userManager.GetUserAsync(User);
			var viewModel = new UserInfoViewModel
			{
				Id = user.Id,
				Username = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email
			};
			return View(viewModel);
		}


		[HttpPost]
		public async Task<IActionResult> UpdateProfile(UserInfoViewModel model)
		{
			if (!ModelState.IsValid)
			{
				foreach (var modelStateKey in ViewData.ModelState.Keys)
				{
					var modelStateVal = ViewData.ModelState[modelStateKey];
					foreach (var error in modelStateVal.Errors)
					{
						var key = modelStateKey;
						var errorMessage = error.ErrorMessage;
						Console.WriteLine($"Error in {key}: {errorMessage}");
					}
				}
				Console.WriteLine("Still!");


				return View("Index", model);
			}

			var user = await userManager.GetUserAsync(User);
			if (user != null)
			{
				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.Email = model.Email;

				unitOfWork.User.Update(user);
				unitOfWork.Save();

				TempData["message"] = $"Updated Successfully!";

				return RedirectToAction("Index");
			}
			else
			{
				Console.WriteLine("null!");
				TempData["message"] = "User not found!";
				return View("Index", model);
			}
		}


		[HttpGet]
		public async Task<IActionResult> DeleteUser(string? id)
		{
			try
			{
				if (id == null)
				{
					Console.WriteLine("NUll");
					return RedirectToAction("404");
				}

				var userToDelete = unitOfWork.User.Get(u => u.Id == id);
				if (userToDelete == null)
				{
					Console.WriteLine("userToDelete NUll");
					return RedirectToAction("404");
				}

				unitOfWork.User.Remove(userToDelete);
								
				unitOfWork.Save();

				// Logout
				await signInManager.SignOutAsync();

				return Redirect("/");

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				TempData["message"] = $"An error occurred while deleting the user: {ex.Message}";
				return RedirectToAction(nameof(Index));
			}
		}
	}
}
