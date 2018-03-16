using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Text;
using ITGA.Common.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace ITGA.Common.Mongo
{
    public static class Extensions
    {
        public static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(configuration.GetSection("mongo"));
            services.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                var mongoConnectionString = options.Value.ConnectionString == "*secret*"
                    ? File.ReadAllText("mongodbConnectionString.key")
                    : options.Value.ConnectionString;
                var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));
                SslProtocols enabledSslProtocol;
                var sslOk = Enum.TryParse(options.Value.Ssl, true, out enabledSslProtocol);
                if (sslOk) settings.SslSettings = new SslSettings { EnabledSslProtocols = enabledSslProtocol };
                return new MongoClient(settings);
            });
            services.AddScoped<IMongoDatabase>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                var client = c.GetService<MongoClient>();
                return client.GetDatabase(options.Value.Database);
            });
            services.AddScoped<IDatabaseInitializer, MongoInitializer>();
            services.AddScoped<IDatabaseSeeder, MongoSeeder>();
        }
    }
}
