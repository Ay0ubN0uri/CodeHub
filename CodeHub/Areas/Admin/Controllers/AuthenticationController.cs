using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthenticationController : Controller
    {
	    public IActionResult Login()
	    {
		    return View();
	    }

	    public IActionResult Register()
	    {
            return View();
	    }
        
        //public IActionResult UnlockUser()
        //{
        //    return View();
        //}
        //public IActionResult ForgetPassword()
        //{
        //    return View();
        //}
        //public IActionResult ResetPassword()
        //{
        //    return View();
        //}
        //public IActionResult Maintenance()
        //{
        //    return View();
        //}
    }
}
