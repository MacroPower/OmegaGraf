// Telegraf //

type TelegrafConfig = {
  Agent: Agent;
  Outputs: Outputs;
  Inputs: Inputs;
};

type Inputs = {
  VSphere: VSphere[];
};

type VSphere = {
  VCenters: string[];
  Username: string;
  Password: string;
  IPAddresses: string[];
  IntSamples: boolean;
  InsecureSkipVerify: boolean;
  ForceDiscover: boolean;
  Interval: string;
  DatastoreMetricExclude?: string[];
  ClusterMetricExclude?: string[];
  DatacenterMetricExclude?: string[];
  MaxQueryMetrics: number;
  CollectConcurrency: number;
  DiscoverConcurrency: number;
  HostMetricExclude?: string[];
  VMMetricExclude?: string[];
};

type Outputs = {
  PrometheusClient: PrometheusClient[];
};

type PrometheusClient = {
  Listen: string;
  StringAsLabel: boolean;
  ExpirationInterval: string;
  Path: string;
};

type Agent = {
  Interval: string;
  RoundInterval: boolean;
  MetricBatchSize: number;
  MetricBufferLimit: number;
  CollectionJitter: string;
  FlushInterval: string;
  FlushJitter: string;
  Precision: string;
  Hostname: string;
  OmitHostname: boolean;
};

// END Telegraf //

// Prometheus //

type StaticConfigs = {
  Targets: string[];
};

type ScrapeConfigs = {
  JobName: string;
  ScrapeInterval: string;
  StaticConfigs: StaticConfigs[];
};

type PrometheusConfig = {
  Global: {
    ScrapeInterval: string;
  };
  ScrapeConfigs: ScrapeConfigs[];
};

// END Prometheus //

// Telegraf //

type BuildConfiguration = {
  Name: string | undefined;
  Image: string;
  Tag: string;
  Ports: number[];
  Binds: { [key: string]: string };
  Parameters: string[];
};

export type Settings = {
  VCSim: {
    BuildConfiguration: BuildConfiguration;
  };
  Telegraf: {
    BuildConfiguration: BuildConfiguration;
    Config: [
      {
        Path: string;
        Data: TelegrafConfig;
      }
    ];
  };
  Prometheus: {
    BuildConfiguration: BuildConfiguration;
    Config: [
      {
        Path: string;
        Data: PrometheusConfig;
      }
    ];
  };
  Grafana: {
    BuildConfiguration: BuildConfiguration;
  };
};

export const defaultSettings: Settings = {
  Grafana: {
    BuildConfiguration: {
      Name: undefined,
      Image: "grafana/grafana",
      Tag: "6.4.3",
      Ports: [3000],
      Binds: {
        "C:/docker/grafana/lib": "/var/lib/grafana"
      },
      Parameters: []
    }
  },
  Telegraf: {
    BuildConfiguration: {
      Name: undefined,
      Image: "telegraf",
      Tag: "latest",
      Ports: [8899],
      Binds: {
        "C:/docker/telegraf": "/etc/telegraf"
      },
      Parameters: []
    },
    Config: [
      {
        Path: "C:/docker/telegraf/telegraf.conf",
        Data: {
          Agent: {
            Interval: "10s",
            RoundInterval: true,
            MetricBatchSize: 1000,
            MetricBufferLimit: 10000,
            CollectionJitter: "0s",
            FlushInterval: "10s",
            FlushJitter: "0s",
            Precision: "",
            Hostname: "OmegaGraf/Telegraf",
            OmitHostname: false
          },
          Outputs: {
            PrometheusClient: [
              {
                Listen: ":8899",
                StringAsLabel: true,
                ExpirationInterval: "600s",
                Path: "/metrics"
              }
            ]
          },
          Inputs: {
            VSphere: [
              {
                VCenters: [
                  "https://og-vcsim:8989/sdk",
                  "https://og-vcsim2:8989/sdk"
                ],
                Username: "user",
                Password: "pass",
                IPAddresses: ["ipv4"],
                IntSamples: true,
                InsecureSkipVerify: true,
                ForceDiscover: true,
                Interval: "60s",
                DatastoreMetricExclude: ["*"],
                ClusterMetricExclude: ["*"],
                DatacenterMetricExclude: ["*"],
                MaxQueryMetrics: 64,
                CollectConcurrency: 1,
                DiscoverConcurrency: 1
              },
              {
                VCenters: [
                  "https://og-vcsim:8989/sdk",
                  "https://og-vcsim2:8989/sdk"
                ],
                Username: "user",
                Password: "pass",
                IPAddresses: ["ipv4"],
                IntSamples: true,
                InsecureSkipVerify: true,
                ForceDiscover: true,
                Interval: "300s",
                HostMetricExclude: ["*"],
                VMMetricExclude: ["*"],
                MaxQueryMetrics: 256,
                CollectConcurrency: 1,
                DiscoverConcurrency: 1
              }
            ]
          }
        }
      }
    ]
  },
  VCSim: {
    BuildConfiguration: {
      Name: "vcsim",
      Image: "macropower/vcsim",
      Tag: "latest",
      Ports: [],
      Binds: {},
      Parameters: [
        "--clusters",
        "2",
        "--data-centers",
        "1",
        "--data-stores",
        "2",
        "--hosts",
        "5",
        "--resource-pools",
        "1",
        "--standalone-host",
        "0",
        "--virtual-machines",
        "20"
      ]
    }
  },
  Prometheus: {
    BuildConfiguration: {
      Name: undefined,
      Image: "prom/prometheus",
      Tag: "latest",
      Ports: [9090],
      Binds: {
        "C:/docker/prometheus/config": "/etc/prometheus",
        "C:/docker/prometheus/data": "/prometheus"
      },
      Parameters: [
        "--config.file=/etc/prometheus/prometheus.yml",
        "--storage.tsdb.path=/prometheus"
      ]
    },
    Config: [
      {
        Path: "C:/docker/prometheus/config/prometheus.yml",
        Data: {
          Global: {
            ScrapeInterval: "30s"
          },
          ScrapeConfigs: [
            {
              JobName: "prometheus",
              ScrapeInterval: "30s",
              StaticConfigs: [
                {
                  Targets: ["localhost:9090"]
                }
              ]
            },
            {
              JobName: "telegraf",
              ScrapeInterval: "60s",
              StaticConfigs: [
                {
                  Targets: ["og-telegraf:8899"]
                }
              ]
            }
          ]
        }
      }
    ]
  }
};
