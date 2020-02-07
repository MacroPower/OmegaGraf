using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Swagger.Services;
using Nancy.TinyIoc;
using Swagger.ObjectModel;
using Swagger.ObjectModel.Builders;

namespace ShopAutomation.API
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public override void Configure(INancyEnvironment environment)
        {
            environment.Diagnostics(true, "password");
            environment.Tracing(enabled: true, displayErrorTraces: true);
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
                version: "alpha",
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
                    Url = "https://github.8451.com/OmegaGraf/"
                },
                schemes: new[] { Schemes.Http, Schemes.Https },
                produces: new[] { "application/json" }
            );

            base.ApplicationStartup(container, pipelines);
        }

        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            AllowAccessToConsumingSite(pipelines);
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