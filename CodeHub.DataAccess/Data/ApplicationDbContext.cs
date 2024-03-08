using CodeHub.Models.Models;
using CodeHub.Utility;
using Microsoft.EntityFrameworkCore;

namespace CodeHub.DataAccess.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
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
