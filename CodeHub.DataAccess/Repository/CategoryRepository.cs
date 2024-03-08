using CodeHub.DataAccess.Data;
using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.Models;

namespace CodeHub.DataAccess.Repository
{
	public class CategoryRepository(ApplicationDbContext db) : Repository<Category>(db), ICategoryRepository
	{
		private ApplicationDbContext _db = db;

		public void Update(Category category)
		{
			_db.Categories.Update(category);
		}

	}
}
