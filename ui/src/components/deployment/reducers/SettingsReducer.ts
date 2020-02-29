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
  GrafanaBuildConfigurationTag
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
    case ActionTypes.PrometheusBuildConfigurationTag:
      return {
        ...state,
        Prometheus: {
          BuildConfiguration: {
            ...state.Prometheus.BuildConfiguration,
            Tag: action.value
          },
          Config: state.Prometheus.Config
        }
      };
    case ActionTypes.TelegrafBuildConfigurationTag:
      return {
        ...state,
        Telegraf: {
          BuildConfiguration: {
            ...state.Telegraf.BuildConfiguration,
            Tag: action.value
          },
          Config: state.Telegraf.Config
        }
      };
    case ActionTypes.GrafanaBuildConfigurationTag:
      return {
        ...state,
        Grafana: {
          BuildConfiguration: {
            ...state.Grafana.BuildConfiguration,
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
