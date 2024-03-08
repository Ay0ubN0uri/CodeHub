
using CodeHub.Models.Models;

namespace CodeHub.Models.ViewModels
{
	public class HomeViewModel
	{
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Product> Products { get; set; }

		public override string ToString()
		{
			return $"{nameof(Categories)}: {Categories}";
		}
	}
}
