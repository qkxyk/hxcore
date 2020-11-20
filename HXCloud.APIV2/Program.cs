using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using HXCloud.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            //var assemblyName = typeof(StartupTest).GetTypeInfo().Assembly.FullName;
            var bulider = Host.CreateDefaultBuilder(args);
            bulider//.ConfigureLogging((context, loggingBuilder) =>
            //{
            //    loggingBuilder.AddFilter("System", LogLevel.Warning);
            //    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
            //})
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.AddFilter("Microsoft", LogLevel.Warning)
                              .AddFilter("System", LogLevel.Warning)
                              .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
            })
            .ConfigureWebHostDefaults(webBulider =>
            {
                //webBulider.UseStartup(assemblyName);
                webBulider.UseStartup<Startup>();
            });
            return bulider;
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //    .ConfigureLogging((context, loggingBuilder) =>
        //    {
        //        loggingBuilder.AddFilter("System", LogLevel.Warning);
        //        loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
        //    }).UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
