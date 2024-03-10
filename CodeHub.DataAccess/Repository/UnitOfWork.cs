using CodeHub.DataAccess.Data;
using CodeHub.DataAccess.Repository.IRepository;
using CodeHub.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeHub.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _db;
		public ICategoryRepository Category { get; }
		public IProductRepository Product { get; }

        public IUserRepository User { get; }

        public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Category = new CategoryRepository(_db);
			Product = new ProductRepository(_db);
			User = new UserRepository(_db);
		}

		public void Save()
		{
			_db.SaveChanges();
		}
    }
}
