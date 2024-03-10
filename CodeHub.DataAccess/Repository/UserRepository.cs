using CodeHub.DataAccess.Data;
using CodeHub.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeHub.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeHub.DataAccess.Repository
{
    public class UserRepository(ApplicationDbContext db) : Repository<User>(db), IUserRepository
    {

    }
}
