using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Auth;
using ITGA.Common.Exceptions;
using ITGA.Services.Identity.Domain.Models;
using ITGA.Services.Identity.Domain.Repositories;
using ITGA.Services.Identity.Domain.Services;

namespace ITGA.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IEncrypter _encrypter;

        public UserService(IUserRepository userRepository, IJwtHandler jwtHandler, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _encrypter = encrypter;
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await _userRepository.GetAsync(email);
            if (user != null)
            {
                throw new ITGAException("email_in_use", $"Email '{email}' is already in use.");
            }

            user = new User(email, name);
            user.SetPassword(password, _encrypter);
            await _userRepository.AddAsync(user);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null)
            {
                throw new ITGAException("invalid_credentials", "Invalid credentials.");
            }
            if (!user.ValidatePassword(password, _encrypter))
            {
                throw new ITGAException("invalid_credentials", "Invalid credentials.");
            }

            return _jwtHandler.Create(user.Id);
        }
    }
}
