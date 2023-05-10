using DataTransferObject;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IUserService
    {
        Task<ApplicationUser> RegisterAsync(ApplicationUserDto userDto);
    }
}
