
using CodeHub.Models.Models;

namespace CodeHub.Models.ViewModels
{
	public class ShopViewModel
	{
		public IEnumerable<Product> Products { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public int TotalPages { get; set; }
		public int ActivePage { get; set; }
		public int? NextPage { get; set; }
		public int? PrevPage { get; set; }
		public int[] SelectedCategories { get; set; }
	}
}
