using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace web_api_managemen_user
{
    public class Program
    {
        private static string _env = "Development";
        public static void Main(string[] args)
        {
            SetEnveronment();
            CreateWebHostBuilder(args).Build().Run();
        }

        private static void SetEnveronment()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false)
                    .Build();
                _env = config.GetSection("Environment").Value;
            }
            catch
            {
                _env = "Development";
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((Hosting_contect,config)=>
        {
            config.AddJsonFile("appsettings.json", false);
            config.AddJsonFile("appsettings."+_env+".json",optional: true);
        })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            //.UseUrls("https://*:8080")
            .UseStartup<Startup>();
    }
}
