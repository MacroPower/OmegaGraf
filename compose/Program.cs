using System;
using Microsoft.AspNetCore.Hosting;
using NLog;
using PowerArgs;

namespace OmegaGraf.Compose
{
    class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Starting up!");
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("OmegaGraf"));

            try
            {
                var parsed = Args.Parse<MyArgs>(args);

                if (parsed.Verbose) Log.SetLogLevel(LogLevel.Info);

                if (parsed.Help)
                {
                    ArgUsage.GenerateUsageFromTemplate<MyArgs>().Write();
                    Environment.Exit(0);
                }
                
                if (parsed.Reset) new Docker().RemoveAllContainers().Wait();

                MetaData.SystemData.SetRoot(parsed.Path);
                logger.Info("Root: " + MetaData.SystemData.GetRoot());

                if (string.IsNullOrWhiteSpace(parsed.Key))
                {
                    var key = KeyDatabase.CreateKey();
                    Console.WriteLine("Your secure code: " + key);
                }
                else
                {
                    KeyDatabase.CreateKey(parsed.Key);
                }

                var urls = 
                    parsed.Host.Length == 0 ? new string[] { "https://0.0.0.0:5001" }
                                            : parsed.Host;

                var host = new WebHostBuilder()
                            .UseKestrel()
                            .UseUrls(urls)
                            .UseStartup<Startup>()
                            .Build();

                host.Run();
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                ArgUsage.GenerateUsageFromTemplate<MyArgs>().Write();
            }
        }
    }
}