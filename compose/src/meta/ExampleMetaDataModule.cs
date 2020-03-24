using Nancy;
using Nancy.Metadata.Modules;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using Nancy.Swagger;
using NLog;
using OmegaGraf.Compose.Config.Prometheus;
using OmegaGraf.Compose.Config.Telegraf;
using Swagger.ObjectModel;
using System.Collections.Generic;

namespace OmegaGraf.Compose.MetaData
{
    public class DefaultSettings
    {
        public Input<Prometheus> Prometheus { get; set; }
        public Input<Telegraf> Telegraf { get; set; }
        public Input Grafana { get; set; }
        public Input VCSim { get; set; }
    }

    public class DefaultModule : NancyModule
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public DefaultModule() : base("/default")
        {
            Get(
                "/",
                args =>
                {
                    logger.Info("Collecting and returning default values");

                    var defaults = new DefaultSettings()
                    {
                        Prometheus = Defaults.Prometheus,
                        Telegraf = Defaults.Telegraf,
                        Grafana = Defaults.Grafana,
                        VCSim = Defaults.VCSim
                    };

                    foreach (var o in defaults.Telegraf.Config[0].Data.Inputs.VSphere)
                    {
                        o.VCenters = new List<string>(){ "" };
                        o.Username = "";
                        o.Password = "";
                    }

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        defaults
                    );
                }, null, "Default"
            );
        }
    }

    public class DefaultMetadataModule : MetadataModule<PathItem>
    {
        public DefaultMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(
                typeof(DefaultSettings),
                typeof(Input)
            );

            Describe["Default"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("Default")
                        .Tag("Info")
                        .Summary("Default Data")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .Response(x => x.Description("Returns an object containing all default settings").Build()))
                );
        }
    }
}