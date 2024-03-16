using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class ShopController(IUnitOfWork unitOfWork) : Controller
	{
		public IActionResult Index(int? pageIndex, int[] cats,string? query = null)
		{
			const int pageSize = 8;
			var pageNumber = pageIndex ?? 1;
			var categories = unitOfWork.Category.GetAll("Products");
			var products = unitOfWork.Product.GetAll("Category");
			if (query is not null)
			{
				products = products.Where(p => p.Name.ToLower().Contains(query));
			}
			if (cats.Length > 0)
			{
				products = products.Where(p => cats.Contains(p.CategoryId));
			}
			int totalProducts = products.Count();
			int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
			if (pageNumber > totalPages || pageNumber <= 0)
			{
				pageNumber = 1;
			}
			products = products
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize);
			int? nextPage = pageNumber == totalPages ? null : pageNumber + 1;
			int? prevPage = pageNumber == 1 ? null : pageNumber - 1;
			return View(new ShopViewModel()
			{
				Products = products,
				Categories = categories,
				TotalPages = totalPages,
				ActivePage = pageNumber,
				NextPage = nextPage,
				PrevPage = prevPage,
				SelectedCategories = cats
			});
		}

		public IActionResult ProductDetails(int? id)
		{
			if (id is null or 0)
			{
				return RedirectToAction("404");
			}

			var product = unitOfWork.Product.Get(p => p.Id == id,"Category");
			if (product is null)
			{
				return RedirectToAction("404");
			}
			return View(product);
		}
	}
}
