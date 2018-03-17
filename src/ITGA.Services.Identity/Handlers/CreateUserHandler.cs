using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Commands;
using ITGA.Common.Commands.Users;
using ITGA.Common.Events.Users;
using ITGA.Common.Exceptions;
using ITGA.Services.Identity.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace ITGA.Services.Identity.Handlers
{
    [UsedImplicitly]
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;

        public CreateUserHandler(IBusClient busClient,
            IUserService userService, 
            ILogger<CreateUser> logger)
        {
            _busClient = busClient;
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(CreateUser command)
        {
            _logger.LogInformation($"Creating user: '{command.Email}' with name: '{command.Name}'.");
            try 
            {
                await _userService.RegisterAsync(command.Email, command.Password, command.Name);
                await _busClient.PublishAsync(new UserCreated(command.Email, command.Name));
                _logger.LogInformation($"User: '{command.Email}' was created with name: '{command.Name}'.");
            }
            catch (ITGAException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email,
                    ex.Message, ex.Code)); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email,
                    ex.Message, "unkown error"));                
            }
        }
    }
}
