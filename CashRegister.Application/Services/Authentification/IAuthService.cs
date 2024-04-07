using CashRegister.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services.Authentification
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
        Task<List<string>> GetUserRoles(User user);
        Task<User> GetByUsername(string username);
    }
}
