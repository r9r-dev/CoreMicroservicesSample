using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Commands;
using ITGA.Common.Commands.Activities;
using ITGA.Common.Events;
using ITGA.Common.Events.Activities;
using ITGA.Common.Mongo;
using ITGA.Common.RabbitMq;
using ITGA.Services.Activities.Domain.Repositories;
using ITGA.Services.Activities.Handlers;
using ITGA.Services.Activities.Repositories;
using ITGA.Services.Activities.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ITGA.Services.Activities
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddLogging();
            services.AddMongoDB(Configuration);
            //services.AddRabbitMq(Configuration);
            services.AddCloudAMQPRabbitMq();
            services.AddScoped<ICommandHandler<CreateActivity>, CreateActivityHandler>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDatabaseSeeder, ActivitiesMongoSeeder>();
            services.AddScoped<IActivityService, ActivityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();
            app.UseMvc();
        }
    }
}
