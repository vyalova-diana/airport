using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Airport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MyTime MT = MyTime.GetMyTime();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseKestrel()
        .UseUrls("http://localhost:7013/")
        .UseStartup<Startup>();
    }
}
