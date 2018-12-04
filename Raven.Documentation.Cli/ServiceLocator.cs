using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Raven.Documentation.Cli.Tasks;

namespace Raven.Documentation.Cli
{
    internal static class ServiceLocator
    {
        private static ServiceProvider Provider { get; set; }

        public static T Resolve<T>()
        {
            return (T)Provider.GetService(typeof(T));
        }

        public static object Resolve(Type t)
        {
            return Provider.GetService(t);
        }

        public static void Configure()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();


            var loggerFactory = new LoggerFactory()
                .AddNLog();

            serviceCollection.AddSingleton(loggerFactory);
            serviceCollection.AddLogging();
            serviceCollection.AddOptions();

            var settings = new Settings();
            configuration.Bind(settings);
            serviceCollection.AddSingleton(settings);

            serviceCollection.AddSingleton<Startup>();
            serviceCollection.AddScoped<ParseArticlesTask>();
            serviceCollection.AddScoped<ParseDocumentationTask>();

            Provider = serviceCollection.BuildServiceProvider();
        }
    }
}
