using CodeHub.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DashboardController(IUnitOfWork unitOfWork) : Controller
    {
		private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IActionResult Index()
		{
			return View();
		}
	}
}
