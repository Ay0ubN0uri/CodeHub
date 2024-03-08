using CodeHub.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeHub.Models.ViewModels
{
	public class ProductViewModel
	{
		public Product Product { get; set; }
		[ValidateNever]
		public IEnumerable<SelectListItem> Categories { get; set; }

		public override string ToString()
		{
			return $"{nameof(Product)}: {Product}, {nameof(Categories)}: {Categories}";
		}
	}
}
