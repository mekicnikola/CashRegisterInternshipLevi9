using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashRegister.Domain.Models;
using CashRegister.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Application.Services.Authentification
{
    public class AuthService : IAuthService
    {
        private readonly CashRegisterDBContext _context;

        public AuthService(CashRegisterDBContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            // password hash
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.UserName == username && u.Password == password);

            return user;
        }

        public async Task<List<string>> GetUserRoles(User user)
        {
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            return roles;
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.UserName == username);

            return user;
        }
    }
}
