using System.Collections.Generic;
using System.IO;
using OmegaGraf.Compose.Config.Grafana;
using OmegaGraf.Compose.Config.Prometheus;
using OmegaGraf.Compose.Config.Telegraf;

namespace OmegaGraf.Compose.MetaData
{
    public static class Defaults
    {
        public static Input<Prometheus> Prometheus => new Input<Prometheus>()
        {
            BuildInput = new BuildConfigurationInput()
            {
                Name  = "prometheus",
                Tag   = "v2.16.0",
                Ports = new Dictionary<int, int>() { { 9090, 9090 } },
                Binds = new Dictionary<string, string>()
                    {
                        {
                            Path.Join("prometheus", "config"), "/etc/prometheus" + SystemData.GetMode()
                        },
                        {
                            Path.Join("prometheus", "data"),   "/prometheus" + SystemData.GetMode()
                        }
                    },
                Parameters = new List<string>()
                    {
                        "--config.file=/etc/prometheus/prometheus.yml",
                        "--storage.tsdb.path=/prometheus",
                        "--storage.tsdb.retention.time=15d"
                    }
            },
            Config = new Config<Prometheus>[]
                {
                    new Config<Prometheus>()
                    {
                        Path = Path.Join("prometheus", "config", "prometheus.yml"),
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

        public static Input<Telegraf> Telegraf => new Input<Telegraf>()
        {
            BuildInput = new BuildConfigurationInput()
            {
                Name  = "telegraf",
                Tag   = "1.13.4",
                Ports = new Dictionary<int, int>() { { 8899, 8899 } },
                Binds = new Dictionary<string, string>()
                    {
                        {
                            "telegraf", "/etc/telegraf" + SystemData.GetMode()
                        }
                    }
            },
            Config = new Config<Telegraf>[]
                {
                    new Config<Telegraf>()
                    {
                        Path = Path.Join("telegraf", "telegraf.conf"),
                        Data = new Telegraf(){
                            Agent = new Agent()
                            {
                                Interval          = "10s",
                                RoundInterval     = true,
                                MetricBatchSize   = 1000,
                                MetricBufferLimit = 10000,
                                CollectionJitter  = "0s",
                                FlushInterval     = "10s",
                                FlushJitter       = "0s",
                                Precision         = "",
                                Hostname          = "OmegaGraf/Telegraf",
                                OmitHostname      = false
                            },
                            Inputs = new Inputs()
                            {
                                Internal = new List<Internal>()
                                {
                                    new Internal()
                                    {
                                        CollectMemstats = true
                                    }
                                },
                                VSphere = new List<VSphere>()
                                {
                                    new VSphere()
                                    {
                                        Interval                = "60s",
                                        VCenters                = new List<string>(){ "https://og-vcsim:8989/sdk", "https://og-vcsim2:8989/sdk" },
                                        Username                = "user",
                                        Password                = "pass",
                                        IPAddresses             = new List<string>(){ "ipv4" },
                                        IntSamples              = true,
                                        InsecureSkipVerify      = true,
                                        ForceDiscover           = true,
                                        DatacenterMetricExclude = new List<string> { "*" },
                                        ClusterMetricExclude    = new List<string> { "*" },
                                        DatastoreMetricExclude  = new List<string> { "*" },
                                        CollectConcurrency      = 1,
                                        DiscoverConcurrency     = 1,
                                        MaxQueryMetrics         = 64
                                    },
                                    new VSphere()
                                    {
                                        Interval            = "300s",
                                        VCenters            = new List<string>(){ "https://og-vcsim:8989/sdk", "https://og-vcsim2:8989/sdk" },
                                        Username            = "user",
                                        Password            = "pass",
                                        IPAddresses         = new List<string>(){ "ipv4" },
                                        IntSamples          = true,
                                        InsecureSkipVerify  = true,
                                        ForceDiscover       = true,
                                        VMMetricExclude     = new List<string> { "*" },
                                        HostMetricExclude   = new List<string> { "*" },
                                        CollectConcurrency  = 1,
                                        DiscoverConcurrency = 1,
                                        MaxQueryMetrics     = 256
                                    }
                                }
                            },
                            Outputs = new Outputs()
                            {
                                PrometheusClient = new List<PrometheusClient>()
                                {
                                    new PrometheusClient()
                                    {
                                        Listen             = ":8899",
                                        Path               = "/metrics",
                                        StringAsLabel      = true,
                                        ExpirationInterval = "600s"
                                    }
                                }
                            }
                        }
                    }
                }
        };

        public static Input Grafana => new Input()
        {
            BuildInput = new BuildConfigurationInput()
            {
                Name  = "grafana",
                Tag   = "6.7.1",
                Ports = new Dictionary<int, int>() { { 3000, 3000 } },
                Binds = new Dictionary<string, string>()
                    {
                        {
                            Path.Join("grafana", "plugins"), "/var/lib/grafana/plugins" + SystemData.GetMode()
                        },
                        {
                            Path.Join("grafana", "grafana.db"), "/var/lib/grafana/grafana.db" + SystemData.GetMode()
                        }
                    }
            }
        };

        public static DataSource GrafanaDataSource => new DataSource()
        {
            ID                = 1,
            OrgID             = 1,
            Name              = "og-prometheus",
            Type              = "prometheus",
            Access            = "proxy",
            URL               = "http://og-prometheus:9090",
            Password          = "",
            User              = "",
            Database          = "",
            BasicAuth         = false,
            BasicAuthUser     = "",
            BasicAuthPassword = "",
            IsDefault         = true,
            JsonData          = null
        };

        public static Input VCSim => new Input()
        {
            BuildInput = new BuildConfigurationInput()
            {
                Name  = "vcsim",
                Tag   = "latest",
                Ports = new Dictionary<int, int>() { },
                Binds = new Dictionary<string, string>() { },
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
