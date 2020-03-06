// Telegraf //

type TelegrafConfig = {
  Agent: Agent;
  Outputs: Outputs;
  Inputs: Inputs;
};

type Inputs = {
  VSphere: VSphere[];
};

export type VSphere = {
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

//

type BuildInput = {
  Name: string | undefined;
  Tag: string;
  Ports: { [key: number]: number };
  Binds: { [key: string]: string };
  Parameters: string[];
};

export type Sim = {
  Active: boolean;
  Quantity: number;
};

export type Settings = {
  VCSim: {
    BuildInput: BuildInput;
  };
  Telegraf: {
    BuildInput: BuildInput;
    Config: [
      {
        Path: string;
        Data: TelegrafConfig;
      }
    ];
  };
  Prometheus: {
    BuildInput: BuildInput;
    Config: [
      {
        Path: string;
        Data: PrometheusConfig;
      }
    ];
  };
  Grafana: {
    BuildInput: BuildInput;
  };
};
