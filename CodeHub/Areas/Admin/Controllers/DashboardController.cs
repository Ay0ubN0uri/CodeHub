using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class DashboardController(IUnitOfWork unitOfWork) : Controller
    {
		private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IActionResult Index()
        {
            var res = _unitOfWork.Category.GetAll("Products").Select(category => new CategoryProductCountViewModel
            {
                CategoryName = category.Name,
                ProductCount = category.Products.Count()
            });

            return View(new DashboardViewModel()
            {
				ProductCount = _unitOfWork.Product.GetAll().Count(),
				CategoryCount = _unitOfWork.Category.GetAll().Count(),
				UserCount = _unitOfWork.User.GetAll().Count(),
				CategoryProductCount = res.ToList()
            });
		}
	}
}
