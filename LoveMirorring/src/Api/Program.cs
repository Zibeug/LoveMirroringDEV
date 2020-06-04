using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            //// This is for in memory database

            ////1. Get the IWebHost which will host this application.
            //var host = CreateHostBuilder(args).Build();

            ////2. Find the service layer within our scope.
            //using (var scope = host.Services.CreateScope())
            //{
            //    //3. Get the instance of BoardGamesDBContext in our services layer
            //    var services = scope.ServiceProvider;
            //    var context = services.GetRequiredService<LoveMirroringContext>();

            //    //4. Call the DataGenerator to create sample data
            //    DataGenerator.Initialize(services);
            //}

            ////Continue to run the application
            //host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls("http://0.0.0.0:5004");
                });
    }
}
