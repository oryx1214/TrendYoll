using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trendyol.Models;

namespace Trendyol.Services.Classes
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool UserLogin(string email, string password)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
            return false;
        }

        public User GetUser(string email)
        {
            return _context.Users.SingleOrDefault(user => user.Email == email);
        }

        public User UserRegister(string name, string surname, string login, string email, string password)
        {
            User user = new User
            {
                Name = name,
                Surname = surname,
                Login = login,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
            };
            return user;
        }
    }
}
