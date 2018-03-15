using System;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ITGA.Common.Commands;
using ITGA.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Configuration.Consumer;
using RawRabbit.Instantiation;
using RawRabbit.Pipe;

namespace ITGA.Common.RabbitMq
{
    public static class Extensions
    {
        //public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
        //    ICommandHandler<TCommand> handler) where TCommand : ICommand
        //    => bus.SubscribeAsync<TCommand>(msg => handler.HandleAsync(msg),
        //        ctx => ctx.UseSubscribeConfiguration(cfg => 
        //            cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TCommand>()))));

        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(msg => handler.HandleAsync(msg),
                ctx => ctx.Properties.Add(PipeKey.ConfigurationAction,
                    (Action<IConsumerConfigurationBuilder>)(cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TCommand>())))));

        //public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus,
        //    IEventHandler<TEvent> handler) where TEvent : IEvent
        //    => bus.SubscribeAsync<TEvent>(msg => handler.HandleAsync(msg),
        //        ctx => ctx.UseSubscribeConfiguration(cfg => 
        //            cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TEvent>()))));

        public static Task WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(msg => handler.HandleAsync(msg),
                ctx => ctx.Properties.Add(PipeKey.ConfigurationAction,
                    (Action<IConsumerConfigurationBuilder>)(cfg => cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TEvent>())))));

        private static string GetQueueName<T>() => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = options
            });
            services.AddSingleton<IBusClient>(_ => client);
        }

        public static void AddCloudAMQPRabbitMq(this IServiceCollection services)
        {
            var connectionString = "icbxoemq:HnIslapjeB8BfhYL0ulFvy2sRS3sBzii@sheep.rmq.cloudamqp.com/icbxoemq";
            var config = ConnectionStringParser.Parse(connectionString);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = config
            });
            services.AddSingleton<IBusClient>(_ => client);
        }
    }
}
