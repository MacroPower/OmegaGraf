import { Settings } from "../settings/Settings";

export type Action = {
  type: string;
  value: string;
};

export function SettingsReducer(state: Settings, action: Action) {
  switch (action.type) {
    case "BuildConfiguration.Image":
      return {
        ...state,
        Prometheus: {
          BuildConfiguration: {
            ...state.Prometheus.BuildConfiguration,
            Image: action.value
          },
          Config: state.Prometheus.Config
        }
      };
    case "BuildConfiguration.Tag":
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
    default:
      throw new Error();
  }
}
