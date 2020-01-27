import { Settings } from './Settings';

export const defaultSettings: Settings = {
  Grafana: {
    BuildConfiguration: {
      Name: undefined,
      Image: '',
      Tag: '',
      Ports: [],
      Binds: {},
      Parameters: []
    }
  },
  Telegraf: {
    BuildConfiguration: {
      Name: undefined,
      Image: '',
      Tag: '',
      Ports: [],
      Binds: {},
      Parameters: []
    },
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
            VSphere: []
          }
        }
      }
    ]
  },
  VCSim: {
    BuildConfiguration: {
      Name: undefined,
      Image: '',
      Tag: '',
      Ports: [],
      Binds: {},
      Parameters: []
    }
  },
  Prometheus: {
    BuildConfiguration: {
      Name: undefined,
      Image: '',
      Tag: '',
      Ports: [],
      Binds: {},
      Parameters: []
    },
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
