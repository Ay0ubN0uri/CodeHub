using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
