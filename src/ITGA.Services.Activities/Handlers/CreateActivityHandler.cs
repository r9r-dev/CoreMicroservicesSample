using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Commands;
using ITGA.Common.Commands.Activities;
using ITGA.Common.Events.Activities;
using ITGA.Common.Exceptions;
using ITGA.Services.Activities.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace ITGA.Services.Activities.Handlers
{
    [UsedImplicitly]
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;
        private readonly IActivityService _activityService;
        private ILogger _logger;

        public CreateActivityHandler(IBusClient busClient, IActivityService activityService, ILogger<CreateActivityHandler> logger)
        {
            _busClient = busClient;
            _activityService = activityService;
            _logger = logger;
        }

        public async Task HandleAsync(CreateActivity command)
        {
            _logger.LogInformation($"Creating activity: {command.Name}");
            
            try
            {
                await _activityService.AddAsync(command.Id, command.UserId, command.Category, command.Name,
                    command.Description, command.CreatedAt);
                await _busClient.PublishAsync(new ActivityCreated(command.Id, command.UserId, command.Category,
                    command.Name, command.Description, command.CreatedAt));
            }
            catch (ITGAException e)
            {
                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, e.Code, e.Message));
                _logger.LogError(e.Message);
            }
            catch (Exception e)
            {
                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, "unexpected_error", e.Message));
                _logger.LogError(e.Message);
            }


            
        }
    }
}
