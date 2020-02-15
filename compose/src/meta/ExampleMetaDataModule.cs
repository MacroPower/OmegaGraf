using Nancy;
using Nancy.Responses.Negotiation;
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
        public ExampleModule() : base("/example")
        {
            Get(
                "/",
                args =>
                {
                    var examples = new ExampleSettings()
                    {
                        Prometheus = Example.Prometheus,
                        Telegraf = Example.Telegraf,
                        Grafana = Example.Grafana,
                        VCSim = Example.VCSim
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