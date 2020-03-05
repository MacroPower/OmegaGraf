import React, { useReducer, useEffect } from 'react';
import { SettingsReducer, ActionTypes } from '../reducers/SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings } from '../../Global';
import TextField from '../inputs/TextField';
import FormView from '../../../views/Form';

export default function NormalForm() {
  const [globalSettings] = UseGlobalSettings();
  const [state, dispatch] = useReducer(SettingsReducer, globalSettings);

  useEffect(() => {
    dispatch({
      type: ActionTypes.Reset,
      value: globalSettings
    });
  }, [globalSettings]);

  return (
    <FormView
      state={state}
      page="normal"
      pageName="Normal"
      title="Deploying Level 2"
      description="Please enter your preferences."
    >
      <AddSystem dispatch={dispatch} state={state} />

      <br />

      <TextField
        dispatch={dispatch}
        label="Telegraf Tag"
        type={ActionTypes.TelegrafBuildConfigurationTag}
        value={state.Telegraf.BuildInput.Tag}
      />

      <TextField
        dispatch={dispatch}
        label="Prometheus Tag"
        type={ActionTypes.PrometheusBuildConfigurationTag}
        value={state.Prometheus.BuildInput.Tag}
      />

      <TextField
        dispatch={dispatch}
        label="Grafana Tag"
        type={ActionTypes.GrafanaBuildConfigurationTag}
        value={state.Grafana.BuildInput.Tag}
      />
    </FormView>
  );
}
