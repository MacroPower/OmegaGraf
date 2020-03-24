using Nancy;
using Nancy.Metadata.Modules;
using Nancy.Responses.Negotiation;
using Nancy.Swagger;
using NLog;
using OmegaGraf.Compose.Config.Prometheus;
using OmegaGraf.Compose.Config.Telegraf;
using Swagger.ObjectModel;
using System.Collections.Generic;

namespace OmegaGraf.Compose.MetaData
{
    public class ExampleSettings
    {
        public Input<Prometheus> Prometheus { get; set; }
        public Input<Telegraf> Telegraf { get; set; }
        public Input Grafana { get; set; }
        public Input VCSim { get; set; }
    }

    public class ExampleModule : NancyModule
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public ExampleModule() : base("/example")
        {
            Get(
                "/",
                args =>
                {
                    logger.Info("Collecting and returning default values");

                    var examples = new ExampleSettings()
                    {
                        Prometheus = Defaults.Prometheus,
                        Telegraf = Defaults.Telegraf,
                        Grafana = Defaults.Grafana,
                        VCSim = Defaults.VCSim
                    };

                    foreach (var o in examples.Telegraf.Config[0].Data.Inputs.VSphere)
                    {
                        o.VCenters = new List<string>(){ "" };
                        o.Username = "";
                        o.Password = "";
                    }

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        examples
                    );
                }, null, "Example"
            );
        }
    }

    public class ExampleMetadataModule : MetadataModule<PathItem>
    {
        public ExampleMetadataModule(ISwaggerModelCatalog modelCatalog)
        {
            modelCatalog.AddModels(
                typeof(ExampleSettings),
                typeof(Input)
            );

            Describe["Example"] =
                desc => desc.AsSwagger(
                    with => with.Operation(
                        op => op.OperationId("Example")
                        .Tag("Info")
                        .Summary("Default Data")
                        .ConsumeMimeType("application/json")
                        .ProduceMimeType("application/json")
                        .Response(x => x.Description("Returns an object containing all default settings").Build()))
                );
        }
    }
}