using System;
using System.Collections.Generic;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Nancy.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using PowerArgs;

namespace OmegaGraf.Compose
{
    public class MyArgs
    {
        [ArgShortcut("-h"), ArgShortcut("--help"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgShortcut("-v"), ArgShortcut("--verbose"), ArgDescription("Enable verbose logging")]
        public bool Verbose { get; set; }

        [ArgShortcut("-l"), ArgShortcut("--test-logs"), ArgDescription("Test logging")]
        public bool Log { get; set; }

        [ArgShortcut("-p"), ArgShortcut("--path"), ArgDescription("Absolute path to store container data. Defaults to current directory."), ArgPosition(1)]
        public string Path { get; set; }

        [ArgShortcut("--host"), ArgDescription("The listen address for this application."), ArgPosition(2), ArgDefaultValue("https://0.0.0.0:5001")]
        public string[] Host { get; set; }
    }

    public class Startup
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Configure(IApplicationBuilder app)
        {
            FlurlHttp.Configure(
                config =>
                {
                    var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    config.JsonSerializer = new NewtonsoftJsonSerializer(settings);
                }
            );

            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                logger.Trace(eventArgs.Exception);
            };

            app.UseOwin(x => x.UseNancy());
        }
    }
}