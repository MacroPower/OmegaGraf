using System.Collections.Generic;
using OmegaGraf.Compose.Config.Prometheus;
using OmegaGraf.Compose.Config.Telegraf;

namespace OmegaGraf.Compose.MetaData
{
    public static class Example
    {
        public static Input<Prometheus> Prometheus = new Input<Prometheus>()
        {
            BuildConfiguration = new BuildConfiguration()
            {
                Image = "prom/prometheus",
                Tag   = "latest",
                Ports = new List<int>(){
                    9090
                },
                Binds = new Dictionary<string, string>()
                {
                    {
                        "C:/docker/prometheus/config", "/etc/prometheus"
                    },
                    {
                        "C:/docker/prometheus/data",   "/prometheus"
                    }
                },
                Parameters = new List<string>()
                {
                    "--config.file=/etc/prometheus/prometheus.yml",
                    "--storage.tsdb.path=/prometheus"
                }
            },
            Config = new Config<Prometheus>[]
            {
                new Config<Prometheus>()
                {
                    Path = "C:/docker/prometheus/config/prometheus.yml",
                    Data = new Prometheus()
                    {
                        Global = new Global()
                        {
                            ScrapeInterval = "5s"
                        },
                        ScrapeConfigs = new List<ScrapeConfigs>()
                        {
                            new ScrapeConfigs()
                            {
                                JobName        = "prometheus",
                                ScrapeInterval = "5s",
                                StaticConfigs  = new List<StaticConfigs>()
                                {
                                    new StaticConfigs()
                                    {
                                        Targets = new string[] {
                                            "localhost:9090"
                                        }
                                    }
                                }
                            },
                            new ScrapeConfigs()
                            {
                                JobName        = "telegraf",
                                ScrapeInterval = "5s",
                                StaticConfigs  = new List<StaticConfigs>()
                                {
                                    new StaticConfigs()
                                    {
                                        Targets = new string[] {
                                            "localhost:8899"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        public static Input<Telegraf> Telegraf = new Input<Telegraf>()
        {
            BuildConfiguration = new BuildConfiguration()
            {
                Image = "telegraf",
                Tag   = "latest",
                Ports = new List<int>(){ 8899 },
                Binds = new Dictionary<string, string>()
                {
                    {
                        "C:/docker/telegraf", "/etc/telegraf"
                    }
                }
            },
            Config = new Config<Telegraf>[]
            {
                new Config<Telegraf>()
                {
                    Path = "C:/docker/telegraf/telegraf.conf",
                    Data = new Telegraf(){
                        Agent = new Agent()
                        {
                            Interval = "10s",
                            RoundInterval = true,
                            MetricBatchSize = 1000,
                            MetricBufferLimit = 10000,
                            CollectionJitter = "0s",
                            FlushInterval = "10s",
                            FlushJitter = "0s",
                            Precision = "",
                            Hostname = "OmegaGraf/Telegraf",
                            OmitHostname = false
                        },
                        Inputs = new Inputs()
                        {
                            VSphere = new List<VSphere>()
                            {
                                new VSphere()
                                {
                                    Interval = "60s",
                                    VCenters = new List<string>(){ "https://vcenter.macro/sdk" },
                                    Username = "monitor@vsphere.macro",
                                    Password = "cP9P7qFPsk5HeRj6CP!",
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
                                    VCenters = new List<string>(){ "https://vcenter.macro/sdk" },
                                    Username = "monitor@vsphere.macro",
                                    Password = "cP9P7qFPsk5HeRj6CP!",
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
                            }
                        },
                        Outputs = new Outputs()
                        {
                            PrometheusClient = new List<PrometheusClient>()
                            {
                                new PrometheusClient()
                                {
                                    Listen = ":8899",
                                    Path = "/metrics",
                                    StringAsLabel = true,
                                    ExpirationInterval = "600s"
                                }
                            }
                        }
                    }
                }
            }
        };

        public static Input Grafana = new Input()
        {
            BuildConfiguration = new BuildConfiguration()
            {
                Image = "grafana/grafana",
                Tag   = "6.4.3",
                Ports = new List<int>(){ 3000 },
                Binds = new Dictionary<string, string>()
                {
                    {
                        "C:/docker/grafana/lib", "/var/lib/grafana"
                    }
                }
            }
        };
    }
}