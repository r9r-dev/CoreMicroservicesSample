using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Api.Handlers;
using ITGA.Common.Events;
using ITGA.Common.Events.Activities;
using ITGA.Common.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace ITGA.Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "ITGA Api Gateway",
                    Version = "v1",
                    Description = "API ITGA",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Ronan Lamour", Email = "", Url = "https://twitter.com/PongRaider" },
                    License = new License { Name = "WTFPL", Url = "http://www.wtfpl.net/" }

                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "ITGA.Api.xml"); 
                c.IncludeXmlComments(xmlPath);
            });

            services.AddRabbitMq(Configuration);
            services.AddScoped<IEventHandler<ActivityCreated>, ActivityCreatedHandler>();
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ITGA Api Gateway v1");
            });
            app.UseMvc();
        }
    }
}
