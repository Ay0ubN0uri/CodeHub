using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController(IUnitOfWork unitOfWork) : Controller
	{
		public IActionResult Index()
		{
			var categories = unitOfWork.Category.GetAll("Products");
			var products = unitOfWork.Product.GetAll("Category");
			foreach (var cat in categories)
			{
				Console.WriteLine(cat.Products);
			}
			return View(new HomeViewModel()
			{
				Categories = categories,
				Products = products
			});
		}
    }
}
