using Microsoft.AspNetCore.Hosting;

namespace OmegaGraf.Compose
{
    class Program
    {
        static void Main()
        {
            var uri = "http://localhost:8888";

            var host = new WebHostBuilder()
                       .UseKestrel()
                       .UseStartup<Startup>()
                       .UseUrls(uri)
                       .Build();

            host.Run();
        }
    }
}