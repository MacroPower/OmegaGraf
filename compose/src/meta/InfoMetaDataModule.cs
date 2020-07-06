using Nancy;
using Nancy.Metadata.Modules;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using Nancy.Swagger;
using Swagger.ObjectModel;

namespace OmegaGraf.Compose.MetaData
{
    public class InfoModule : NancyModule
    {
        public InfoModule()
        {
            this.Get(
                "/auth", args =>
                {
                    return this.Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Authenticated = KeyDatabase.ValidateKey(this.Request.Headers.Authorization)
                        }
                    );
                }, null, "Auth"
            );

            this.Get(
                "/info", args =>
                {
                    this.RequiresAuthentication();

                    return this.Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Globals.Version,
                            Authenticated = true
                        }
                    );
                }, null, "Info"
            );
        }
    }

    public class InfoMetadataModule : MetadataModule<PathItem>
    {
        public InfoMetadataModule(ISwaggerModelCatalog _)
        {
            this.Describe["Info"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("Info")
                        .Tag("Info")
                        .Summary("Deployment Info")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .SecurityRequirement(SecuritySchemes.ApiKey)
                        .Response(x => x.Description("Deployment Info").Build()))
                );

            this.Describe["Auth"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("Auth")
                        .Tag("Info")
                        .Summary("Auth status")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .Response(x => x.Description("Shows the status of the provided API key").Build()))
                );
        }
    }
}
