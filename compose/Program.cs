using System;
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
                    if (parsed.Reset)
                    {
                        var docker = new Docker();
                        var containers = docker.ListContainers().Result;
                        var tasks = new List<Task>();

                        foreach (var container in containers)
                        {
                            foreach (var name in container.Names)
                            {
                                if (name.StartsWith("/og-"))
                                {
                                    logger.Info("Removing " + name + ", " + container.ID);

                                    tasks.Add(
                                        Task.Run(async () => {
                                                await docker.StopContainer(container.ID);
                                                await docker.RemoveContainer(container.ID);
                                            }));
                                }
                            }
                        }

                        Task t = Task.WhenAll(tasks);
                        t.Wait();
                    }

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
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                ArgUsage.GenerateUsageFromTemplate<MyArgs>().Write();
            }
        }
    }
}