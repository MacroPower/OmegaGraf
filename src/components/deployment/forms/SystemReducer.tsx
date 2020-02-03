import { Settings, VSphere } from '../../settings/Settings';

export type Action = {
  type: 'Address' | 'Username' | 'Password' | 'Add' | 'Remove';
  index: number;
  current: string[];
  value: any;
};

const newVCenters = (state: Settings, current: string[]): Settings => {
  const inputs = state.Telegraf.Config[0].Data.Inputs.VSphere
  
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

export function SystemReducer(state: Settings, action: Action): Settings {
  const current = action.current;

  switch (action.type) {
    case 'Add':
      return newVCenters(state, [...current, '']);
    case 'Remove':
      const ns = current;
      ns.splice(action.index, 1);
      return newVCenters(state, [...ns]);
    case 'Address':
      current[action.index] = action.value;
      return newVCenters(state, [...current]);
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
    default:
      throw new Error();
  }
}
