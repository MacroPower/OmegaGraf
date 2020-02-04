import { Settings, VSphere } from '../settings/Settings';

export type Action = any;

/*
{
  type: 'Address' | 'Username' | 'Password' | 'Add' | 'Remove' | 'BuildConfiguration.Image' | 'BuildConfiguration.Tag' | 'reset';
  index: number | undefined
  current: string[] | undefined;
  value: any;
};
*/
const newVCenters = (state: Settings, current: VSphere[]): Settings => {
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
              VSphere: current
            }
          }
        }
      ]
    }
  };
};

const newVCenterEndpoints = (state: Settings, current: string[]): Settings => {
  const inputs = state.Telegraf.Config[0].Data.Inputs.VSphere;

  const vs1 = {
    ...inputs[0],
    VCenters: current
  };

  const vs2 = {
    ...inputs[1],
    VCenters: current
  };

  return newVCenters(state, [vs1, vs2]);
};

const newVCenterUser = (state: Settings, current: string): Settings => {
  const inputs = state.Telegraf.Config[0].Data.Inputs.VSphere;

  const vs1 = {
    ...inputs[0],
    Username: current
  };

  const vs2 = {
    ...inputs[1],
    Username: current
  };

  return newVCenters(state, [vs1, vs2]);
};

const newVCenterPassword = (state: Settings, current: string): Settings => {
  const inputs = state.Telegraf.Config[0].Data.Inputs.VSphere;

  const vs1 = {
    ...inputs[0],
    Password: current
  };

  const vs2 = {
    ...inputs[1],
    Password: current
  };

  return newVCenters(state, [vs1, vs2]);
};

export function SettingsReducer(state: Settings, action: Action) {
  switch (action.type) {
    case 'Add':
      const removedBlanks = action.current.filter((v: string) => v);
      return newVCenters(state, [...removedBlanks, '']);
    case 'Remove':
      const ns = action.current;
      ns.splice(action.index + 1, 1);
      return newVCenters(state, [...ns]);
    case 'Address':
      action.current[action.index] = action.value;
      return newVCenterEndpoints(state, [...action.current]);
    case 'Username':
      const username: string = action.value;
      return newVCenterUser(state, username);
    case 'Password':
      const password: string = action.value;
      return newVCenterPassword(state, password);
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
