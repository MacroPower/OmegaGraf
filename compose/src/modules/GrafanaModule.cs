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

namespace OmegaGraf.Compose.Modules
{
    public class GrafanaModule : NancyModule
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public GrafanaModule() : base("/grafana")
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
                    var bind = this.Bind<Input>();

                    var bc = bind.BuildInput.ToBuildConfiguration("grafana/grafana");

                    var uuid = new Runner().AddConfig(
                        // This file only exists to ensure permissions are correct.
                        new Config<string>()
                        {
                            Path = Path.Join("grafana", "plugins", ".OmegaGraf"),
                            Data = "",
                        }
                    ).Build(bc);

                    return this.Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Container = uuid
                        }
                    );
                }, null, "DeployGrafana"
            );

            this.Post(
                "/datasource", args =>
                {
                    logger.Info("Adding default Grafana datasource");

                    var bind = this.Bind<GrafanaInput>();

                    var g = new Grafana("http://localhost:" + bind.Port);

                    g.AddDataSource(Defaults.GrafanaDataSource).Wait();
                    g.Dispose();

                    return this.Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new
                        {
                            Result = "Complete."
                        }
                    );
                }, null, "DeployGrafanaDataSource"
            );

            this.Post(
                "/dashboards", args =>
                {
                    logger.Info("Adding default Grafana dashboards");

                    var bind = this.Bind<GrafanaInput>();

                    var g = new Grafana("http://localhost:" + bind.Port);

                    var filepath = Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, "grafana/dashboards/");

                    try
                    {
                        foreach (var file in Directory.GetFiles(filepath, "*.json"))
                        {
                            logger.Info("Adding Grafana dashboard from file : " + file);

                            var json = File.ReadAllText(file);
                            g.AddDashboard(
                                new Models.Grafana.Dashboard()
                                {
                                    DashboardData = JsonConvert.DeserializeObject(json)
                                }).Wait();
                        }
                    }
                    catch (IOException ex)
                    {
                        logger.Error(ex, "Error reading a dashboard file");
                        throw;
                    }
                    catch (System.Exception ex)
                    {
                        logger.Error(ex, "Unknown Exception");
                        throw;
                    }

                    g.Dispose();

                    return this.Negotiate.WithMediaRangeModel(
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
                typeof(BuildConfigurationInput),
                typeof(Config<Grafana>),
                typeof(Input<Grafana>)
            );

            this.Describe["DeployGrafana"] =
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
                                            Example = Defaults.Grafana
                                        }
                                    )
                                    .Build()
                        ).Response(x => x.Description("Container UUID").Build()))
                );

            this.Describe["DeployGrafanaDataSource"] =
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

            this.Describe["DeployGrafanaDashboard"] =
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
