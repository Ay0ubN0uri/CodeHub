using CodeHub.DataAccess.Data;
using CodeHub.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

namespace CodeHub.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		public readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;
		public Repository(ApplicationDbContext db)
		{
			_db = db;
			dbSet = _db.Set<T>();
		}

		public IEnumerable<T> GetAll(string? includeProperties = null)
		{
			IQueryable<T> query = dbSet;
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return query.ToList();
		}

		public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
		{
			IQueryable<T> query = dbSet;
			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return query.Where(filter).FirstOrDefault();
		}

		public void Add(T entity)
		{
			dbSet.Add(entity);
		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			dbSet.RemoveRange(entities);
		}
	}
}
