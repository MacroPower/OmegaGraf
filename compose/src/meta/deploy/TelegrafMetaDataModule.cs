using Nancy;
using Nancy.Metadata.Modules;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using Nancy.Swagger;
using OmegaGraf.Compose.Config.Telegraf;
using Swagger.ObjectModel;

namespace OmegaGraf.Compose.MetaData
{
    public class TelegrafModule : NancyModule
    {
        public TelegrafModule() : base("/telegraf")
        {
            this.RequiresAuthentication();

            Get(
                "/{id}",
                args =>
                {
                    return HttpStatusCode.OK;
                }, null, "Info"
            );

            Post(
                "/",
                args =>
                {
                    Input<Telegraf> bind = (this).Bind<Input<Telegraf>>();

                    var bc = bind.BuildInput.ToBuildConfiguration("telegraf");

                    var uuid = new Runner().AddTomlConfig(x => x.LowerCase, bind.Config)
                               .Build(bc);

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Container = uuid
                        }
                    );
                }, null, "DeployTelegraf"
            );

            Post(
                "/sim",
                args =>
                {
                    Input bind = (this).Bind<Input>();

                    var bc = bind.BuildInput.ToBuildConfiguration("macropower/vcsim");

                    var uuid = new Runner().Build(bc);

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Container = uuid
                        }
                    );
                }, null, "DeployVCSim"
            );
        }
    }

    public class TelegrafMetadataModule : MetadataModule<PathItem>
    {
        public TelegrafMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(
                typeof(Telegraf),
                typeof(Agent),
                typeof(Outputs),
                typeof(Inputs),
                typeof(PrometheusClient),
                typeof(VSphere),
                typeof(BuildConfiguration),
                typeof(Config<Telegraf>),
                typeof(Input<Telegraf>)
            );

            Describe["DeployTelegraf"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("DeployTelegraf")
                        .Tag("Deploy")
                        .Summary("Deploy Telegraf")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .SecurityRequirement(SecuritySchemes.ApiKey)
                        .BodyParameter(
                            para =>
                                para.Name("Build")
                                    .Schema(
                                        new Schema()
                                        {
                                            Example = Example.Telegraf
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );

            Describe["DeployVCSim"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("DeployVCSim")
                        .Tag("Deploy")
                        .Summary("Deploy VC Simulator")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .SecurityRequirement(SecuritySchemes.ApiKey)
                        .BodyParameter(
                            para =>
                                para.Name("Build")
                                    .Schema(
                                        new Schema()
                                        {
                                            Example = Example.VCSim
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );
        }
    }
}