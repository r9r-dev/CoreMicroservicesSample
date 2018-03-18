using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Auth;

namespace ITGA.Services.Identity.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string name);
        Task<JsonWebToken> LoginAsync(string email, string password);
    }
}
