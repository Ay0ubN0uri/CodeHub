using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.Models;
using CodeHub.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeHub.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment _webHostEnvironment) : Controller
	{
		public IActionResult Index()
		{
			//var p = new Product()
			//{
			//	Name = "Zibber - Consulting Business WordPress Theme + RTL",
			//	Description =
			//		"Zibber is a business multi-Purpose WordPress theme for all kinds of business consulting services, such as business consulting, marketing consulting, advertising consulting, financial advisors, insurance brokers, investment consultants, accountant services, HR consulting and many more. . Zibber WordPress Theme for consulting services is responsive and retina ready. This Theme build with worlds most popular responsive CSS framework Bootstrap 4, Elementor, HTML5, CSS3, jQuery and so many modern technology. In few words, it is powerful, easy to use multi-purpose Theme. From the first glance, you will be impressed with its trendy and energetic design with smooth transitions and animations. Upon purchase, you will benefit from 3 different homepage designs with 4+ header styles, so that you can always have a lot of options to customize your site. Use its options for your website development!. I hope that I have covered everything but if there is something that you would like to know then I am happy to help out. ",
			//	Downloads = 10,
			//	Version = "1.1",
			//	ImageUrl = "img.jpg",
			//	LogoUrl = "logo.png",
			//	SourceCodeUrl = "source.zip",
			//	Category = unitOfWork.Category.GetAll().ToList()[0]
			//};
			//unitOfWork.Product.Add(p);
			//unitOfWork.Save();
			var products = unitOfWork.Product.GetAll("Category");
			return View(products);
		}

		public IActionResult Upsert(int? id)
		{
			var categories = unitOfWork.Category.GetAll().Select(u => new SelectListItem()
			{
				Value = u.Id.ToString(),
				Text = u.Name
			});
			if (id is null or 0)
			{
				return View(new ProductViewModel()
				{
					Categories = categories,
					Product = new Product()
					{
						Id = 0
					}
				});
			}

			var product = unitOfWork.Product.Get(p => p.Id == id, "Category");
			if (product == null) return RedirectToAction("404");
			return View(new ProductViewModel()
			{
				Categories = categories,
				Product = product
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Product product, IFormFile? logo, IFormFile? image)//, IFormFile? source)
		{
			var categories = unitOfWork.Category.GetAll().Select(u => new SelectListItem()
			{
				Value = u.Id.ToString(),
				Text = u.Name
			});
			var wwwRootPath = _webHostEnvironment.WebRootPath;
			var uploadPath = Path.Combine(wwwRootPath, @"uploads");
			var productViewModel = new ProductViewModel()
			{
				Categories = categories,
				Product = new Product()
				{
					Id = 0
				}
			};
			if (ModelState.IsValid)
			{

				if (product.Id == 0)
				{
					// create new product
					if (logo == null)
					{
						ModelState.AddModelError("Product.LogoUrl", "Product logo is required.");
						return View(productViewModel);
					}
					if (image == null)
					{
						ModelState.AddModelError("Product.ImageUrl", "Product image is required.");
						return View(productViewModel);
					}
					//if (source == null)
					//{
					//	ModelState.AddModelError("Product.SourceCodeUrl", "Product source code is required.");
					//	return View(productViewModel);
					//}
					var logoFileName = Guid.NewGuid() + Path.GetExtension(logo.FileName);
					var imageFileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
					//var sourceFileName = Guid.NewGuid() + Path.GetExtension(source.FileName);
					SaveFile(logo, uploadPath, logoFileName);
					SaveFile(image, uploadPath, imageFileName);
					//SaveFile(source, uploadPath, sourceFileName);
					product.LogoUrl = logoFileName;
					product.ImageUrl = imageFileName;
					//product.SourceCodeUrl = product.SourceCodeUrl;
					unitOfWork.Product.Add(product);
					TempData["message"] = "Product created successfully";
				}
				else
				{
					// update product
					var productToUpdate = unitOfWork.Product.Get(p => p.Id == product.Id, "Category");
					if (productToUpdate == null) return RedirectToAction("404");
					if (!string.IsNullOrEmpty(productToUpdate.LogoUrl) && logo != null)
					{
						// delete the old logo
						DeleteOldFile(uploadPath, productToUpdate.LogoUrl);
						SaveFile(logo, uploadPath, productToUpdate.LogoUrl);
					}
					if (!string.IsNullOrEmpty(productToUpdate.ImageUrl) && image != null)
					{
						// delete the old image
						DeleteOldFile(uploadPath, productToUpdate.ImageUrl);
						SaveFile(image, uploadPath, productToUpdate.ImageUrl);
					}
					//if (!string.IsNullOrEmpty(productToUpdate.SourceCodeUrl) && source != null)
					//{
					//	// delete the old source
					//	DeleteOldFile(uploadPath, productToUpdate.SourceCodeUrl);
					//	SaveFile(source, uploadPath, productToUpdate.SourceCodeUrl);
					//}

					//product.LogoUrl = productToUpdate.LogoUrl;
					//product.ImageUrl = productToUpdate.ImageUrl;
					//product.SourceCodeUrl = productToUpdate.SourceCodeUrl;
					productToUpdate.Name = product.Name;
					productToUpdate.Description = product.Description;
					productToUpdate.CategoryId = product.CategoryId;
					productToUpdate.Price = product.Price;
					productToUpdate.Version = product.Version;
					unitOfWork.Product.Update(productToUpdate);
					TempData["message"] = "Product updated successfully";
				}
				unitOfWork.Save();
				return RedirectToAction(nameof(Index));
			}
			return View(productViewModel);
		}

		private static void DeleteOldFile(string uploadPath, string fileUrl)
		{
			var oldLogoPath = Path.Combine(uploadPath, fileUrl);
			if (System.IO.File.Exists(oldLogoPath))
			{
				System.IO.File.Delete(oldLogoPath);
			}
		}

		private static void SaveFile(IFormFile file, string uploadPath, string fileFileName)
		{
			using (var fileStream = new FileStream(Path.Combine(uploadPath, fileFileName), FileMode.Create))
			{
				file.CopyTo(fileStream);
			}
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("404");
			}

			var product = unitOfWork.Product.Get(c => c.Id == id);
			if (product == null)
			{
				return RedirectToAction("404");
			}
			var wwwRootPath = _webHostEnvironment.WebRootPath;
			var uploadPath = Path.Combine(wwwRootPath, @"uploads");
			DeleteOldFile(uploadPath, product.LogoUrl);
			DeleteOldFile(uploadPath, product.ImageUrl);
			DeleteOldFile(uploadPath, product.SourceCodeUrl);
			unitOfWork.Product.Remove(product);
			unitOfWork.Save();
			TempData["message"] = "Product has been deleted";
			return RedirectToAction(nameof(Index));
		}
	}
}
