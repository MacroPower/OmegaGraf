using System.IO;
using Nancy;
using Nancy.Metadata.Modules;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using Nancy.Swagger;
using Newtonsoft.Json;
using NLog;
using Swagger.ObjectModel;

namespace OmegaGraf.Compose.MetaData
{
    public class GrafanaModule : NancyModule
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public GrafanaModule() : base("/grafana")
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
                    var g = new Grafana("http://localhost:3000");

                    logger.Info("Adding default datasource");

                    g.AddDataSource(Example.GrafanaDataSource).Wait();
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
                "/dashboards",
                args =>
                {
                    var g = new Grafana("http://localhost:3000");

                    string filepath = Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "grafana/dashboards/");

                    try
                    {
                        foreach (var file in Directory.GetFiles(filepath, "*.json"))
                        {
                            string json = File.ReadAllText(file);
                            var dash = JsonConvert.DeserializeObject(json);
                            logger.Info("Adding Grafana dashboard from file : " + file);
                            g.AddDashboard(dash).Wait();
                        }
                    }
                    catch (IOException ex)
                    {
                        logger.Error(ex, "Error reading a dashboard file");
                        throw;
                    }

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
                        .SecurityRequirement(SecuritySchemes.ApiKey)
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
                        .SecurityRequirement(SecuritySchemes.ApiKey)
                        .Response(x => x.Description("Container UUID").Build()))
                );

            Describe["DeployGrafanaDashboard"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("DeployGrafanaDashboard")
                        .Tag("Config")
                        .Summary("Add Grafana Dashboard")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .SecurityRequirement(SecuritySchemes.ApiKey)
                        .Response(x => x.Description("Container UUID").Build()))
                );
        }
    }
}