import { Settings, VSphere } from '../settings/Settings';

export type Action = any;

/*{
  type: 'Address' | 'Username' | 'Password' | 'Add' | 'Remove' | 'BuildConfiguration.Image' | 'BuildConfiguration.Tag' | 'reset';
  index: number
  current: string[];
  value: any;
};*/

const newVCenters = (state: Settings, current: any): Settings => {
  return {
    ...state,
    Telegraf: {
      BuildConfiguration: state.Telegraf.BuildConfiguration,
      Config: [
        {
          ...state.Telegraf.Config[0],
          Data: {
            ...state.Telegraf.Config[0].Data,
            Inputs: {
              VSphere: [
                {
                  ...state.Telegraf.Config[0].Data.Inputs.VSphere[0],
                  VCenters: current.systems,
                  Username: current.username,
                  Password: current.password
                },
                {
                  ...state.Telegraf.Config[0].Data.Inputs.VSphere[1],
                  VCenters: current.systems,
                  Username: current.username,
                  Password: current.password
                }
              ]
            }
          }
        }
      ]
    }
  };
};

export function SettingsReducer(state: Settings, action: Action): Settings {
  switch (action.type) {
    case 'vCenter':
      return newVCenters(state, action.value);
    case 'BuildConfiguration.Image':
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
    case 'BuildConfiguration.Tag':
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
    case 'reset':
      return { ...action.value };
    default:
      throw new Error();
  }
}
