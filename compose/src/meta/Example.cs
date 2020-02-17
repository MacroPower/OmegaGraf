using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using NLog;
using OmegaGraf.Compose.Config.Grafana;
using OmegaGraf.Compose.Config.Prometheus;
using OmegaGraf.Compose.Config.Telegraf;

namespace OmegaGraf.Compose.MetaData
{
    public static class Example
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        public static string Mode
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return ":rw";
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "";
                }

                var e = new NotSupportedException();
                logger.Error(e, "OS Platform is not supported");
                throw e;
            }
        }

        public static string Root
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "/docker/";
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "C:/docker/";
                }

                throw new NotSupportedException();
            }
        }

        public static Input<Prometheus> Prometheus = new Input<Prometheus>()
        {
            BuildConfiguration = new BuildConfiguration()
            {
                Image = "prom/prometheus",
                Tag   = "latest",
                Ports = new List<int>(){ 9090 },
                Binds = new Dictionary<string, string>()
                {
                    {
                        Path.Join(Root, "prometheus/config"), "/etc/prometheus" + Mode
                    },
                    {
                        Path.Join(Root, "prometheus/data"),   "/prometheus" + Mode
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
                    Path = Path.Join(Root, "prometheus/config", "/prometheus.yml"),
                    Data = new Prometheus()
                    {
                        Global = new Global()
                        {
                            ScrapeInterval = "30s"
                        },
                        ScrapeConfigs = new List<ScrapeConfigs>()
                        {
                            new ScrapeConfigs()
                            {
                                JobName        = "prometheus",
                                ScrapeInterval = "30s",
                                StaticConfigs  = new List<StaticConfigs>()
                                {
                                    new StaticConfigs()
                                    {
                                        Targets = new string[]
                                        {
                                            "localhost:9090"
                                        }
                                    }
                                }
                            },
                            new ScrapeConfigs()
                            {
                                JobName        = "telegraf",
                                ScrapeInterval = "60s",
                                StaticConfigs  = new List<StaticConfigs>()
                                {
                                    new StaticConfigs()
                                    {
                                        Targets = new string[]
                                        {
                                            "og-telegraf:8899"
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
                        Path.Join(Root, "telegraf"), "/etc/telegraf" + Mode
                    }
                }
            },
            Config = new Config<Telegraf>[]
            {
                new Config<Telegraf>()
                {
                    Path = Path.Join(Root, "telegraf", "/telegraf.conf"),
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
                                    VCenters = new List<string>(){ "https://og-vcsim:8989/sdk", "https://og-vcsim2:8989/sdk" },
                                    Username = "user",
                                    Password = "pass",
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
                                    VCenters = new List<string>(){ "https://og-vcsim:8989/sdk", "https://og-vcsim2:8989/sdk" },
                                    Username = "user",
                                    Password = "pass",
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
                        Path.Join(Root, "grafana/lib"), "/var/lib/grafana" + Mode
                    }
                }
            }
        };

        public static DataSource GrafanaDataSource = new DataSource()
        {
            ID = 1,
            OrgID = 1,
            Name = "og-prometheus",
            Type = "prometheus",
            Access = "proxy",
            URL = "http://og-prometheus:9090",
            Password = "",
            User = "",
            Database = "",
            BasicAuth = false,
            BasicAuthUser = "",
            BasicAuthPassword = "",
            IsDefault = true,
            JsonData = null
        };

        public static Input VCSim = new Input()
        {
            BuildConfiguration = new BuildConfiguration()
            {
                Name  = "vcsim",
                Image = "macropower/vcsim",
                Tag   = "latest",
                Ports = new List<int>(){ },
                Binds = new Dictionary<string, string>(){ },
                Parameters = new List<string>()
                {
                    "--clusters", "2",
                    "--data-centers", "1",
                    "--data-stores", "2",
                    "--hosts", "5",
                    "--resource-pools", "1",
                    "--standalone-host", "0",
                    "--virtual-machines", "20",
                }
            }
        };
    }
}