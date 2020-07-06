using System;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Swagger.Services;
using Nancy.TinyIoc;
using OmegaGraf.Compose;
using Swagger.ObjectModel;
using Swagger.ObjectModel.Builders;

namespace ShopAutomation.API
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public override void Configure(INancyEnvironment environment)
        {
            var enabled = Globals.Development;
            var key = Guid.NewGuid().ToString().Replace("-", null);

            if (enabled)
            {
                Console.WriteLine("Development mode is enabled.");
                Console.WriteLine("Diagnostic interface: http://+/_Nancy");
                Console.WriteLine("Password: " + key);
            }

            environment.Diagnostics(enabled, key);
            environment.Tracing(enabled: enabled, displayErrorTraces: enabled);
            base.Configure(environment);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/", "ui"));
            base.ConfigureConventions(nancyConventions);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            this.Conventions.ViewLocationConventions.Add((viewName, model, context) =>
            {
                return string.Concat("ui/", viewName);
            });

            SwaggerMetadataProvider.SetInfo(
                title: "OmegaGraf",
                version: Globals.Version,
                desc: "OmegaGraf-Compose",
                contact: new Contact()
                {
                    EmailAddress = "colvinjm@mail.uc.edu",
                    Name = "Jacob Colvin"
                }
            );

            var securitySchemeBuilder = new ApiKeySecuritySchemeBuilder();

            securitySchemeBuilder.IsInHeader();
            securitySchemeBuilder.Name("Authorization");
            SwaggerMetadataProvider.SetSecuritySchemeBuilder(securitySchemeBuilder, "ApiKey");

            SwaggerMetadataProvider.SetSwaggerRoot(
                externalDocumentation: new ExternalDocumentation
                {
                    Description = "GitHub",
                    Url = "https://github.com/OmegaGraf/OmegaGraf"
                },
                schemes: new[] { Schemes.Http, Schemes.Https },
                produces: new[] { "application/json" }
            );

            base.ApplicationStartup(container, pipelines);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            var configuration =
                new StatelessAuthenticationConfiguration(
                    ctx =>
                    {
                        var apiKey = ctx.Request.Headers.Authorization;

                        return KeyDatabase.GetClaimFromApiKey(apiKey);
                    });

            AllowAccessToConsumingSite(pipelines);

            StatelessAuthentication.Enable(pipelines, configuration);
        }

        static void AllowAccessToConsumingSite(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(
                x =>
                {
                    x.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    x.Response.Headers.Add("Access-Control-Allow-Headers", "*");
                    x.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS");
                }
            );
        }
    }
}
