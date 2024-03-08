using Bogus;
using CodeHub.Models.Models;

namespace CodeHub.Utility
{
	public class FakeDataGenerator
	{
		public static List<Category> GenerateFakeCategoriesData(int count)
		{
			var faker = new Faker<Category>()
				.RuleFor(e => e.Name, f => f.Commerce.Categories(1)[0])
				.RuleFor(e => e.Description, f => f.Lorem.Lines(1));

			return faker.Generate(count);
		}
		public static List<Product> GenerateFakeProductsData(int count, Category category)
		{
			var faker = new Faker<Product>()
				.RuleFor(e => e.Name, f => f.Commerce.Product())
				.RuleFor(e => e.Description, f => f.Lorem.Lines(1))
				.RuleFor(e => e.Version, f => f.System.Version().ToString())
				.RuleFor(e => e.Downloads, f => f.Random.UInt(min: 0U, max: 20000))
				.RuleFor(e => e.ImageUrl, "img.jpg")
				.RuleFor(e => e.LogoUrl, "logo.png")
				.RuleFor(e => e.SourceCodeUrl, "source.zip")
				.RuleFor(e => e.Price, f => f.Random.Double(20, 200))
				.RuleFor(e => e.Category, category);

			return faker.Generate(count);
		}
	}
}