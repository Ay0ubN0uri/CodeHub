using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.Models;
using CodeHub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        // Primary constructor with IUnitOfWork injected
        public UserController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: UserController
        public ActionResult Index()
        {
            /*var users = _unitOfWork.User.GetAll();
            return View(users);*/

            var adminRole = "Admin";
            var nonAdminUsers = _userManager.GetUsersInRoleAsync(adminRole).Result;
            var allUsers = _unitOfWork.User.GetAll().Where(u => !nonAdminUsers.Contains(u));
            return View(allUsers);
        }


        [HttpGet]
        public IActionResult Delete([FromQuery] string? id)
        {
            Console.WriteLine("id: " + id);

            try
            {
                if (id == null)
                {
                    return RedirectToAction("404");
                }

                var userToDelete = _unitOfWork.User.Get(u => u.Id == id);
                if (userToDelete == null)
                {
                    return RedirectToAction("404");
                }
                
                _unitOfWork.User.Remove(userToDelete);
                _unitOfWork.Save();
                TempData["message"] = "User has been deleted";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = $"An error occurred while deleting the user: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
