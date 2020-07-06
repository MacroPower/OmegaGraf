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

            this.Get(
                "/{id}", args =>
                {
                    return HttpStatusCode.OK;
                }, null, "Info"
            );

            this.Post(
                "/", args =>
                {
                    var bind = this.Bind<Input<Telegraf>>();

                    var bc = bind.BuildInput.ToBuildConfiguration("telegraf");

                    var uuid = new Runner().AddTomlConfig(x => x.LowerCase, bind.Config)
                               .Build(bc);

                    return this.Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Container = uuid
                        }
                    );
                }, null, "DeployTelegraf"
            );

            this.Post(
                "/sim", args =>
                {
                    var bind = this.Bind<Input>();

                    var bc = bind.BuildInput.ToBuildConfiguration("macropower/vcsim");

                    var uuid = new Runner().Build(bc);

                    return this.Negotiate.WithMediaRangeModel(
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
                typeof(BuildConfigurationInput),
                typeof(Internal),
                typeof(Config<Telegraf>),
                typeof(Input<Telegraf>)
            );

            this.Describe["DeployTelegraf"] =
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
                                            Example = Defaults.Telegraf
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );

            this.Describe["DeployVCSim"] =
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
                                            Example = Defaults.VCSim
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );
        }
    }
}
