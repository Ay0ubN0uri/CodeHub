using CodeHub.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHub.DataAccess.Data
{
	public static class RolesInitializer
	{
		public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(new IdentityRole("Admin"));
			}
			if (!await roleManager.RoleExistsAsync("User"))
			{
				await roleManager.CreateAsync(new IdentityRole("User"));
			}
		}

		public static async Task SeedAdminAsync(UserManager<User> userManager)
		{
			if (userManager.FindByEmailAsync("admin@admin.com").Result == null)
			{
				User admin = new User
				{
					UserName = "admin",
					FirstName = "adminF",
					LastName = "adminL",
					Email = "admin@admin.com"
				};
				IdentityResult result = await userManager.CreateAsync(admin, "AdminPassword123!");
				if (!result.Succeeded)
				{
					
					foreach (var error in result.Errors)
					{
						Console.WriteLine(error.Description);
					}
					return; 
				}
				await userManager.AddToRoleAsync(admin, "Admin");
			}
		}

	}
}
