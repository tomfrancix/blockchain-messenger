using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using sakurai.Core.Factory;
using sakurai.Core.Helper;
using sakurai.Core.Processor;
using sakurai.Core.Service;
using sakurai.Interface.IFactory;
using sakurai.Interface.IHelper;
using sakurai.Interface.IProcessor;
using sakurai.Interface.IService;
using sakurai.Objects;
using TextToAsciiArt;

namespace sakurai
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ");
            var writer = new ArtWriter();
            var setting = new ArtSetting
            {
                ConsoleSpeed = 100,
                SpaceWidth = 0
            };
            writer.WriteConsole("SAKURAI", setting);
            Console.WriteLine(" ");

            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider?.GetService<Sakurai>()?.Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services
                .AddTransient<INetworkService, NetworkService>()
                .AddTransient<ITimestampHelper, TimestampHelper>()
                .AddTransient<IBlockFactory, BlockFactory>()
                .AddTransient<IHashFactory, HashFactory>()
                .AddTransient<IBlockProcessor, BlockProcessor>()
                .AddLogging()
                .AddTransient<Sakurai>();

            Console.WriteLine("Configuring services:");
            foreach (var service in services)
            {
                Console.WriteLine(" - " + service.ServiceType);
            }

            return services;
        }
    }
}
