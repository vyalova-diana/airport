using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BusClasses;

namespace BusApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = new FileCSV();
            file.ClearAll();
            var blog = new BusLog();
            blog.ClearAll();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseKestrel()
        .UseUrls("http://localhost:7002/")
        .UseStartup<Startup>();
    }
}
