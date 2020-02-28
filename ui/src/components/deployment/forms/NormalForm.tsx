import React, { useReducer, useEffect } from 'react';
import { SettingsReducer } from '../SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings } from '../../Global';
import TextField from '../inputs/TextField';
import FormView from '../../../views/Form';

export default function NormalForm() {
  const [globalSettings] = UseGlobalSettings();
  const [state, dispatch] = useReducer(SettingsReducer, globalSettings);

  useEffect(() => {
    dispatch({
      type: 'reset',
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

      <TextField
        dispatch={dispatch}
        label="BuildConfiguration.Image"
        type="BuildConfiguration.Image"
        value={state.Prometheus.BuildConfiguration.Image}
      />
    </FormView>
  );
}
