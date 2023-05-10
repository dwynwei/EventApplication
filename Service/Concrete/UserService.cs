using DataAccessLayer;
using DataTransferObject;
using Helper.Hasher;
using Model;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Concrete
{
    public class UserService : IUserService
    {
        private readonly EventDBContext _context;

        public UserService(EventDBContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> RegisterAsync(ApplicationUserDto userDto)
        {
            string salt = PasswordHasher.GenerateSalt();

            string hashedPassword = PasswordHasher.HashPassword(userDto.Password, salt);   

            var user = new ApplicationUser
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
