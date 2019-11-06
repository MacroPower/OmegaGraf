using Nancy;
using Nancy.Extensions;
using Nancy.Metadata.Modules;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Swagger;
using Newtonsoft.Json;
using OmegaGraf.Compose.Config.Grafana;
using Swagger.ObjectModel;

namespace OmegaGraf.Compose.MetaData
{
    public class GrafanaModule : NancyModule
    {
        public GrafanaModule() : base("/grafana")
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
                    Input bind = (this).Bind<Input>();

                    var uuid = new Runner().Build(bind.BuildConfiguration);

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Container = uuid
                        }
                    );
                }, null, "DeployGrafana"
            );

            Post(
                "/datasource",
                args =>
                {
                    DataSource bind = (this).Bind<DataSource>();

                    var g = new Grafana("http://localhost:3000");

                    g.AddDataSource(bind).Wait();

                    g.Dispose();

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Result = "Complete."
                        }
                    );
                }, null, "DeployGrafanaDataSource"
            );

            Post(
                "/dashboard",
                args =>
                {
                    string json = this.Request.Body.AsString();

                    var g = new Grafana("http://localhost:3000");

                    var dash = JsonConvert.DeserializeObject(json);

                    g.AddDashboard(dash).Wait();

                    g.Dispose();

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Result = "Complete."
                        }
                    );
                }, null, "DeployGrafanaDashboard"
            );
        }
    }

    public class GrafanaMetadataModule : MetadataModule<PathItem>
    {
        public GrafanaMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(
                typeof(Grafana),
                typeof(string),
                typeof(BuildConfiguration),
                typeof(Config<Grafana>),
                typeof(Input<Grafana>)
            );

            Describe["DeployGrafana"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("DeployGrafana")
                        .Tag("Deploy")
                        .Summary("Deploy Grafana")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .BodyParameter(
                            para =>
                                para.Name("Build")
                                    .Schema(
                                        new Schema()
                                        {
                                            Example = Example.Grafana
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );

            Describe["DeployGrafanaDataSource"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("DeployGrafanaDataSource")
                        .Tag("Config")
                        .Summary("Add Grafana DataSource")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .BodyParameter(
                            para =>
                                para.Name("Build")
                                    .Schema(
                                        new Schema()
                                        {
                                            Example = Example.GrafanaDataSource
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );

            Describe["DeployGrafanaDashboard"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("DeployGrafanaDashboard")
                        .Tag("Config")
                        .Summary("Add Grafana Dashboard")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .BodyParameter(
                            para =>
                                para.Name("Build")
                                    .Description("Dynamic JSON body from Grafana's dashboard export")
                                    .Schema(
                                        new Schema()
                                        {
                                            Example = @"{""dashboard"": { ... }}"
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );
        }
    }
}