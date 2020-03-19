using Nancy;
using Nancy.Responses.Negotiation;
using NLog;
using OmegaGraf.Compose.Config.Grafana;
using OmegaGraf.Compose.Config.Prometheus;
using OmegaGraf.Compose.Config.Telegraf;
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

                    examples.Telegraf.Config[0].Data.Inputs.VSphere = new List<VSphere>()
                    {
                        new VSphere()
                        {
                            Interval = "60s",
                            VCenters = new List<string>(){ "" },
                            Username = "",
                            Password = "",
                            IPAddresses = new List<string>(){ "ipv4" },
                            IntSamples = true,
                            InsecureSkipVerify = true,
                            ForceDiscover = true,
                            DatacenterMetricExclude = new List<string> { "*" },
                            ClusterMetricExclude = new List<string> { "*" },
                            DatastoreMetricExclude = new List<string> { "*" },
                            CollectConcurrency = 1,
                            DiscoverConcurrency = 1,
                            MaxQueryMetrics = 64
                        },
                        new VSphere()
                        {
                            Interval = "300s",
                            VCenters = new List<string>(){ "" },
                            Username = "",
                            Password = "",
                            IPAddresses = new List<string>(){ "ipv4" },
                            IntSamples = true,
                            InsecureSkipVerify = true,
                            ForceDiscover = true,
                            VMMetricExclude = new List<string> { "*" },
                            HostMetricExclude = new List<string> { "*" },
                            CollectConcurrency = 1,
                            DiscoverConcurrency = 1,
                            MaxQueryMetrics = 256
                        }
                    };

                    return Negotiate.WithMediaRangeModel(
                        new MediaRange("application/json"),
                        examples
                    );
                }, null, "Example"
            );
        }
    }
}