import { Settings } from '../../settings/Settings';

export const newVCenters = (state: Settings, current: any): Settings => {
  return {
    ...state,
    Telegraf: {
      BuildInput: state.Telegraf.BuildInput,
      Config: [
        {
          ...state.Telegraf.Config[0],
          Data: {
            ...state.Telegraf.Config[0].Data,
            Inputs: {
              ...state.Telegraf.Config[0].Data.Inputs,
              VSphere: [
                {
                  ...state.Telegraf.Config[0].Data.Inputs.VSphere[0],
                  VCenters: current.systems,
                  Username: current.username,
                  Password: current.password,
                },
                {
                  ...state.Telegraf.Config[0].Data.Inputs.VSphere[1],
                  VCenters: current.systems,
                  Username: current.username,
                  Password: current.password,
                },
              ],
            },
          },
        },
      ],
    },
  };
};
