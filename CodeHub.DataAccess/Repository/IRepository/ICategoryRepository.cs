using CodeHub.Models.Models;

namespace CodeHub.DataAccess.Repository.IRepository
{
	public interface ICategoryRepository : IRepository<Category>
	{
		void Update(Category category);
	}
}
