import { Settings } from './Settings';

const defaultBuildConfiguration = {
  BuildInput: {
    Name: undefined,
    Tag: '',
    Ports: { 0: 0 },
    Binds: {},
    Parameters: []
  }
}


export const defaultSettings: Settings = {
  VCSim: defaultBuildConfiguration,
  Grafana: defaultBuildConfiguration,
  Telegraf: {
    ...defaultBuildConfiguration,
    Config: [
      {
        Path: '',
        Data: {
          Agent: {
            Interval: '',
            RoundInterval: false,
            MetricBatchSize: 0,
            MetricBufferLimit: 0,
            CollectionJitter: '',
            FlushInterval: '',
            FlushJitter: '',
            Precision: '',
            Hostname: '',
            OmitHostname: false
          },
          Outputs: {
            PrometheusClient: []
          },
          Inputs: {
            Internal: [
              {
                CollectMemstats: false
              }
            ],
            VSphere: [
              {
                VCenters: [''],
                Username: '',
                Password: '',
                IPAddresses: [],
                IntSamples: false,
                InsecureSkipVerify: false,
                ForceDiscover: false,
                Interval: '',
                DatastoreMetricExclude: [],
                ClusterMetricExclude: [],
                DatacenterMetricExclude: [],
                MaxQueryMetrics: 0,
                CollectConcurrency: 0,
                DiscoverConcurrency: 0
              },
              {
                VCenters: [''],
                Username: '',
                Password: '',
                IPAddresses: [],
                IntSamples: false,
                InsecureSkipVerify: false,
                ForceDiscover: false,
                Interval: '',
                HostMetricExclude: [],
                VMMetricExclude: [],
                MaxQueryMetrics: 0,
                CollectConcurrency: 0,
                DiscoverConcurrency: 0
              }
            ]
          }
        }
      }
    ]
  },
  Prometheus: {
    ...defaultBuildConfiguration,
    Config: [
      {
        Path: '',
        Data: {
          Global: {
            ScrapeInterval: ''
          },
          ScrapeConfigs: []
        }
      }
    ]
  }
};
