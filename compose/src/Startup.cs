using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Nancy.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OmegaGraf.Compose
{
    public class Startup
    {
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

            app.UseOwin(x => x.UseNancy());
        }
    }
}