using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController(IUnitOfWork unitOfWork) : Controller
	{
		public IActionResult Index()
		{
			var categories = unitOfWork.Category.GetAll();
			return View(categories);
		}

		public IActionResult Upsert(int? id)
		{
			if (id is null or 0)
			{
				return View(new Category()
				{
					Id = 0,
					Name = "",
					Description = ""
				});
			}
			else
			{
				var category = unitOfWork.Category.Get(cat => cat.Id == id);
				if (category == null) return RedirectToAction("404");
				return View(category);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Category category)
		{
			if (ModelState.IsValid)
			{
				if (category.Id == 0)
				{
					unitOfWork.Category.Add(category);
					TempData["message"] = "Category created successfully";
				}
				else
				{
					unitOfWork.Category.Update(category);
					TempData["message"] = "Category updated successfully";
				}
				unitOfWork.Save();
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("404");
			}

			var category = unitOfWork.Category.Get(c => c.Id == id);
			if (category == null)
			{
				return RedirectToAction("404");
			}
			unitOfWork.Category.Remove(category);
			unitOfWork.Save();
			TempData["message"] = "Category has been deleted";
			return RedirectToAction(nameof(Index));
		}
	}
}
