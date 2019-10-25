using Nancy;
using Nancy.Metadata.Modules;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Swagger;
using OmegaGraf.Compose.Config;
using Swagger.ObjectModel;
using System.Collections.Generic;

namespace OmegaGraf.Compose.MetaData
{
    public class PrometheusModule : NancyModule
    {
        public PrometheusModule() : base("/prometheus")
        {
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
                    Input<Prometheus> bind = (this).Bind<Input<Prometheus>>();

                    var uuid = new Runner().AddYamlConfig(bind.Config).Build(bind.BuildConfiguration);

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Container = uuid
                        }
                    );
                }, null, "DeployPrometheus"
            );
        }
    }

    public class PrometheusMetadataModule : MetadataModule<PathItem>
    {
        public PrometheusMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(
                typeof(Prometheus),
                typeof(ScrapeConfigs),
                typeof(Global),
                typeof(StaticConfigs),
                typeof(BuildConfiguration),
                typeof(Config<Prometheus>),
                typeof(Input<Prometheus>)
            );

            Describe["DeployPrometheus"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("DeployPrometheus")
                        .Tag("Deploy")
                        .Summary("Deploy Prometheus")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .BodyParameter(
                            para =>
                                para.Name("Build")
                                    .Schema(
                                        new Schema()
                                        {
                                            Example = Example.Prometheus
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );
        }
    }
}