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

type TelegrafConfig = {};

type VCSimConfig = {};

type GrafanaConfig = {};

type Config = {
  Path: string,
  Data: PrometheusConfig | TelegrafConfig | VCSimConfig | GrafanaConfig
};

type BuildConfiguration = {
  Image: string,
  Tag: string,
  Ports: number[],
  Binds: { [key: string]: string },
  Parameters: string[]
};

export type Setting = {
  BuildConfiguration: BuildConfiguration,
  Config: Config[]
};

export type Settings = {
  Prometheus: {
    BuildConfiguration: BuildConfiguration,
    Config: Config[]
  }
};

export const defaultSettings: Settings = {
  Prometheus: {
    BuildConfiguration: {
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
