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

                if (parsed.Verbose)
                {
                    Log.SetLogLevel(LogLevel.Info);
                }

                if (parsed.Log)
                {
                    logger.Trace("Trace Error Example");
                    logger.Info("Info Error Example");
                    logger.Warn("Warn Error Example");
                    logger.Error("Error Error Example");
                    logger.Fatal("Fatal Error Example");
                }

                if (parsed.Help)
                {
                    ArgUsage.GenerateUsageFromTemplate<MyArgs>().Write();
                }
                else
                {
                    Console.WriteLine("Your secure code: " + Guid.NewGuid().ToString());

                    var host = new WebHostBuilder()
                            .UseKestrel()
                            .UseStartup<Startup>()
                            .Build();

                    host.Run();
                }
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                ArgUsage.GenerateUsageFromTemplate<MyArgs>().Write();
            }
        }
    }
}