using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Exceptions;
using ITGA.Services.Identity.Domain.Services;
using RawRabbit.Operations.Publish.Middleware;

namespace ITGA.Services.Identity.Domain.Models
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string Name { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected User()
        {

        }

        public User(string email, string name)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ITGAException("empty_user_email", "User email cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ITGAException("empty_user_name", "Username cannot be empty.");
            }

            Id = Guid.NewGuid();
            Email = email.ToLowerInvariant();
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password, IEncrypter encrypter)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ITGAException("empty_password", "Password cannont be empty.");
            }

            Salt = encrypter.GetSalt();
            Password = encrypter.GetHash(password, Salt);
        }

        public bool ValidatePassword(string password, IEncrypter encrypter) =>
            Password.Equals(encrypter.GetHash(password, Salt));

    }
}
