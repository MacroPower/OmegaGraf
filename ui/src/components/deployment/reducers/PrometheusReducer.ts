import { Settings } from '../../settings/Settings';

export const newPrometheusScrapeInterval = (
  state: Settings,
  current: string,
  type: 'short' | 'long',
): Settings => {
  let config;

  if (type === 'short') {
    config = {
      Global: {
        ScrapeInterval: current,
      },
      ScrapeConfigs: [
        {
          ...state.Prometheus.Config[0].Data.ScrapeConfigs[0],
          ScrapeInterval: current,
        },
        {
          ...state.Prometheus.Config[0].Data.ScrapeConfigs[1],
        },
      ],
    };
  } else {
    config = {
      ...state.Prometheus.Config[0].Data,
      ScrapeConfigs: [
        {
          ...state.Prometheus.Config[0].Data.ScrapeConfigs[0],
        },
        {
          ...state.Prometheus.Config[0].Data.ScrapeConfigs[1],
          ScrapeInterval: current,
        },
      ],
    };
  }

  return {
    ...state,
    Prometheus: {
      ...state.Prometheus,
      Config: [
        {
          ...state.Prometheus.Config[0],
          Data: config,
        },
      ],
    },
  };
};
