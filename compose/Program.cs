﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    System.Environment.Exit(0);
                }
                
                if (parsed.Reset) new Docker().RemoveAllContainers().Wait();

                MetaData.Example.Root = parsed.Path;
                logger.Info("Root: " + MetaData.Example.Root);

                Console.WriteLine("Your secure code: " + Guid.NewGuid().ToString());

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