
namespace CodeHub.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
		IProductRepository Product { get; }

        IUserRepository User { get; }

        void Save();
	}
}
