using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

namespace WebCoreApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var location = Assembly.GetEntryAssembly().Location;
            var directory = Path.GetDirectoryName(location);

            var host = new WebHostBuilder()
                .UseKestrel()
              
                .UseContentRoot(directory)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
