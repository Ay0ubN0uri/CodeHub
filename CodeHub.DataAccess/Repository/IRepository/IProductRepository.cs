using CodeHub.Models.Models;

namespace CodeHub.DataAccess.Repository.IRepository
{
	public interface IProductRepository: IRepository<Product>
	{
		void Update(Product product);
	}
}
