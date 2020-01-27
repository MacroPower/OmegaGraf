using Nancy;
using Nancy.Metadata.Modules;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Swagger;
using Swagger.ObjectModel;
using OmegaGraf.Compose.Config.Grafana;
using OmegaGraf.Compose.Config.Prometheus;
using OmegaGraf.Compose.Config.Telegraf;

namespace OmegaGraf.Compose.MetaData
{
    public class ExampleSettings
    {
        public Input<Prometheus> Prometheus;
        public Input<Telegraf> Telegraf;
        public Input Grafana;
        public Input VCSim;
    }

    public class ExampleModule : NancyModule
    {
        public ExampleModule() : base("/example")
        {
            Get(
                "/",
                args =>
                {
                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        new ExampleSettings()
                        {
                            Prometheus = Example.Prometheus,
                            Telegraf = Example.Telegraf,
                            Grafana = Example.Grafana,
                            VCSim = Example.VCSim
                        }
                    );
                }, null, "Example"
            );
        }
    }
}