import { Settings } from '../../settings/Settings';
import { newPrometheusScrapeInterval } from './PrometheusReducer';
import { newVCenters } from './VCReducer';

export enum ActionTypes {
  Reset,
  VCenterInputs,
  PrometheusConfigDataScrapeIntervalShort,
  PrometheusConfigDataScrapeIntervalLong,
  PrometheusBuildConfigurationTag,
  PrometheusRetentionTime,
  TelegrafBuildConfigurationTag,
  GrafanaBuildConfigurationTag,
  GrafanaBuildConfigurationPort,
}

export type Action = {
  type: ActionTypes;
  value: any;
};

export function SettingsReducer(state: Settings, action: Action): Settings {
  switch (action.type) {
    case ActionTypes.VCenterInputs: {
      return newVCenters(state, action.value);
    }
    case ActionTypes.PrometheusConfigDataScrapeIntervalShort: {
      return newPrometheusScrapeInterval(state, action.value, 'short');
    }
    case ActionTypes.PrometheusConfigDataScrapeIntervalLong: {
      return newPrometheusScrapeInterval(state, action.value, 'long');
    }
    case ActionTypes.PrometheusRetentionTime: {
      const parameters = [...state.Prometheus.BuildInput.Parameters];
      const r = /^--storage\.tsdb\.retention\.time=.*$/g;
      const index = parameters.findIndex((p) => p.match(r));
      const newParameter = '--storage.tsdb.retention.time=' + action.value;

      if (index === -1) {
        parameters.push(newParameter);
      } else {
        parameters[index] = newParameter;
      }

      return {
        ...state,
        Prometheus: {
          BuildInput: {
            ...state.Prometheus.BuildInput,
            Parameters: parameters,
          },
          Config: state.Prometheus.Config,
        },
      };
    }
    case ActionTypes.GrafanaBuildConfigurationPort: {
      var port = parseInt(action.value, 10);
      return {
        ...state,
        Grafana: {
          BuildInput: {
            ...state.Grafana.BuildInput,
            Ports: { 3000: port },
          },
        },
      };
    }
    case ActionTypes.PrometheusBuildConfigurationTag: {
      return {
        ...state,
        Prometheus: {
          BuildInput: {
            ...state.Prometheus.BuildInput,
            Tag: action.value,
          },
          Config: state.Prometheus.Config,
        },
      };
    }
    case ActionTypes.TelegrafBuildConfigurationTag: {
      return {
        ...state,
        Telegraf: {
          BuildInput: {
            ...state.Telegraf.BuildInput,
            Tag: action.value,
          },
          Config: state.Telegraf.Config,
        },
      };
    }
    case ActionTypes.GrafanaBuildConfigurationTag: {
      return {
        ...state,
        Grafana: {
          BuildInput: {
            ...state.Grafana.BuildInput,
            Tag: action.value,
          },
        },
      };
    }
    case ActionTypes.Reset: {
      return { ...action.value };
    }
    default: {
      throw new Error();
    }
  }
}
