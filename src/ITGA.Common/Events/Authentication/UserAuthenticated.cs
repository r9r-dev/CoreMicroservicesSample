using System;
using System.Collections.Generic;
using System.Text;

namespace ITGA.Common.Events.Authentication
{
    public class UserAuthenticated : IEvent
    {
        public string Email { get; }

        protected UserAuthenticated()
        {

        }

        public UserAuthenticated(string email)
        {
            Email = email;
        }
    }
}
