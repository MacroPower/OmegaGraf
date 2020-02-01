import { Settings } from './Settings';

const defaultBuildConfiguration = {
  BuildConfiguration: {
    Name: undefined,
    Image: '',
    Tag: '',
    Ports: [],
    Binds: {},
    Parameters: []
  }
}


export const defaultSettings: Settings = {
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
            VSphere: []
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
