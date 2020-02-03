import { Settings } from '../settings/Settings';

export type Action = any;

/*
{
  type: 'Address' | 'Username' | 'Password' | 'Add' | 'Remove' | 'BuildConfiguration.Image' | 'BuildConfiguration.Tag' | 'reset';
  index: number | undefined
  current: string[] | undefined;
  value: any;
};
*/

const newVCenters = (state: Settings, current: string[]): Settings => {
  const inputs = state.Telegraf.Config[0].Data.Inputs.VSphere;

  const vs1 = {
    ...inputs[0],
    VCenters: current
  };

  const vs2 = {
    ...inputs[1],
    VCenters: current
  };

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
              VSphere: [vs1, vs2]
            }
          }
        }
      ]
    }
  };
};

export function SettingsReducer(state: Settings, action: Action) {
  switch (action.type) {
    case 'Add':
      return newVCenters(state, [...action.current, '']);
    case 'Remove':
      const ns = action.current;
      ns.splice(action.index + 1, 1);
      return newVCenters(state, [...ns]);
    case 'Address':
      action.current[action.index] = action.value;
      return newVCenters(state, [...action.current]);
    case 'Username':
      const username: string = action.value;
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
                      Username: username
                    }
                  ]
                }
              }
            }
          ]
        }
      };
    case 'Password':
      const password: string = action.value;
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
                      Password: password
                    }
                  ]
                }
              }
            }
          ]
        }
      };
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
