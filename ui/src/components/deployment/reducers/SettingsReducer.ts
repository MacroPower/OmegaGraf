import { Settings } from '../../settings/Settings';
import { newPrometheusScrapeInterval } from './PrometheusReducer';
import { newVCenters } from './VCReducer';

export enum ActionTypes {
  Reset,
  vCenterInputs,
  PrometheusConfigDataScrapeIntervalShort,
  PrometheusConfigDataScrapeIntervalLong,
  PrometheusBuildConfigurationTag,
  TelegrafBuildConfigurationTag,
  GrafanaBuildConfigurationTag,
  GrafanaBuildConfigurationPort
}

export type Action = {
  type: ActionTypes;
  value: any;
};

export function SettingsReducer(state: Settings, action: Action): Settings {
  switch (action.type) {
    case ActionTypes.vCenterInputs:
      return newVCenters(state, action.value);
    case ActionTypes.PrometheusConfigDataScrapeIntervalShort:
      return newPrometheusScrapeInterval(state, action.value, 'short');
    case ActionTypes.PrometheusConfigDataScrapeIntervalLong:
      return newPrometheusScrapeInterval(state, action.value, 'long');
    case ActionTypes.GrafanaBuildConfigurationPort:
      var port = parseInt(action.value, 10);
      return {
        ...state,
        Grafana: {
          BuildInput: {
            ...state.Grafana.BuildInput,
            Ports: { 3000: port }
          }
        }
      };
    case ActionTypes.PrometheusBuildConfigurationTag:
      return {
        ...state,
        Prometheus: {
          BuildInput: {
            ...state.Prometheus.BuildInput,
            Tag: action.value
          },
          Config: state.Prometheus.Config
        }
      };
    case ActionTypes.TelegrafBuildConfigurationTag:
      return {
        ...state,
        Telegraf: {
          BuildInput: {
            ...state.Telegraf.BuildInput,
            Tag: action.value
          },
          Config: state.Telegraf.Config
        }
      };
    case ActionTypes.GrafanaBuildConfigurationTag:
      return {
        ...state,
        Grafana: {
          BuildInput: {
            ...state.Grafana.BuildInput,
            Tag: action.value
          }
        }
      };
    case ActionTypes.Reset:
      return { ...action.value };
    default:
      throw new Error();
  }
}
