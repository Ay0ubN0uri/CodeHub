using CodeHub.DataAccess.Data;
using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.Models;

namespace CodeHub.DataAccess.Repository
{
	public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db),IProductRepository
	{
		public void Update(Product product)
		{
			product.UpdatedAt = DateTime.UtcNow;
			_db.Products.Update(product);
		}
	}
}
