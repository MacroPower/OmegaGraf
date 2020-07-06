using System;
using Microsoft.AspNetCore.Builder;
using Flurl.Http;
using Flurl.Http.Configuration;
using Nancy.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;

namespace OmegaGraf.Compose
{
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
