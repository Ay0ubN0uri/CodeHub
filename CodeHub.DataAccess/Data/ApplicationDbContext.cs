using CodeHub.Models.Models;
using CodeHub.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CodeHub.DataAccess.Data 
{
	public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		/*public DbSet<User> Users { get; set; }*/

		/*protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<User>().Ignore(u => u.PhoneNumber);
			builder.Entity<User>().Ignore(u => u.NormalizedUserName);
			builder.Entity<User>().Ignore(u => u.PhoneNumberConfirmed);
			builder.Entity<User>().Ignore(u => u.NormalizedEmail);
			builder.Entity<User>().Ignore(u => u.ConcurrencyStamp);
			builder.Entity<User>().Ignore(u => u.LockoutEnd);
			builder.Entity<User>().Ignore(u => u.LockoutEnabled);
			builder.Entity<User>().Ignore(u => u.AccessFailedCount);
			builder.Entity<User>().Ignore(u => u.SecurityStamp);
			builder.Entity<User>().Ignore(u => u.UserName);
			builder.Entity<User>().Ignore(u => u.TwoFactorEnabled);
			builder.Entity<User>().Ignore(u => u.EmailConfirmed);
		}*/

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();

			if (Categories != null && !Categories.Any())
			{
				Categories.AddRange(FakeDataGenerator.GenerateFakeCategoriesData(10));
				SaveChanges();
			}

			if (Products != null && !Products.Any())
			{
				if (Categories != null)
				{
					Products.AddRange(FakeDataGenerator.GenerateFakeProductsData(10, Categories.ToList()[0]));
					SaveChanges();
				}
			}
		}
	}
}
