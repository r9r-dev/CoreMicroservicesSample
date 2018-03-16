using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Commands.Activities;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace ITGA.Api.Controllers
{
    [Route("[controller]")]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;

        public ActivitiesController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        /// <summary>
        /// Create an activity based on his category.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>back url containing command id</returns>
        /// <response code="202">If the command received</response>
        [HttpPost("")]
        [ProducesResponseType(typeof(Guid), 202)]
        public async Task<IActionResult> Post([FromBody] CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            await _busClient.PublishAsync(command);
            return Accepted($"activities/{command.Id}");
        }
    }
}
