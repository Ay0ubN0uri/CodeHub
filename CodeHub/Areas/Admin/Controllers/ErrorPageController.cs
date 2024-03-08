using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ErrorPageController : Controller
    {
        [Route("400")]
        public IActionResult ErrorPage400()
        {
            return View();
        }
        [Route("401")]
        public IActionResult ErrorPage401()
        {
            return View();
        }
        [Route("403")]
        public IActionResult ErrorPage403()
        {
            return View();
        }
        [Route("404")]
        public IActionResult ErrorPage404()
        {
            return View();
        }
        [Route("500")]
        public IActionResult ErrorPage500()
        {
            return View();
        }
        [Route("503")]
        public IActionResult ErrorPage503()
        {
            return View();
        }
    }
}
