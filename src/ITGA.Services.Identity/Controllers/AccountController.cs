using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Commands.Authentication;
using ITGA.Services.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITGA.Services.Identity.Controllers
{
    [Route("")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUser command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));
    }
}
