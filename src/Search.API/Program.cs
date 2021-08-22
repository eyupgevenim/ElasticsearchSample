namespace Search.API
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Search.API.Data;
    using Search.API.Extensions;
    using Search.Elastic.Abstraction;
    using Search.Elastic.Types;
    using Serilog;
    using System;
    using System.IO;

    public class Program
    {
        const string AppName = "Search.API";
        readonly static IConfiguration configuration = GetConfiguration();

        public static int Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                var host = BuildWebHost(configuration, args);
                Log.Information("Applying migrations ({ApplicationContext})...", AppName);

                host
                    .MigrateElasticsearchContext<IElasticContext>((context, services) =>
                    {
                        var logger = services.GetService<ILogger<IElasticContext>>();
                        var elasticsearchOptions = services.GetService<IOptions<ElasticsearchSettings>>();
                        new ApplicationElasticsearchContextSeed()
                            .SeedAsync(elasticsearchOptions.Value, context, logger)
                            .Wait();
                    });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
               WebHost.CreateDefaultBuilder(args)
               .CaptureStartupErrors(false)
               .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
               .UseStartup<Startup>()
               .UseContentRoot(Directory.GetCurrentDirectory())
               //.UseSerilog()
               .Build();

        static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/Log.txt", rollingInterval: RollingInterval.Day)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

    }
}
