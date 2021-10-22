using _09_auth.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _09_auth.Data
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetUserByEmail(string email);
    }
}
