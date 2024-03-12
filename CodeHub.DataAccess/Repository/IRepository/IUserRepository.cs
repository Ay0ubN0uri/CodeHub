using CodeHub.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeHub.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
		void Update(User user);
	}
}
