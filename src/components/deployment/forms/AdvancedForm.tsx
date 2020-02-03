import React from 'react';
import { Action } from '../SettingsReducer';
import TextField from '../inputs/TextField';
import AddSystem from '../inputs/AddSystem';

export default function AdvancedForm(props: {
  dispatch: React.Dispatch<Action>;
  state: any;
}) {
  const { dispatch, state } = props;

  return (
    <>
      <AddSystem dispatch={dispatch} state={state} />

      <TextField
        dispatch={dispatch}
        label="BuildConfiguration.Image"
        type="BuildConfiguration.Image"
        value={state.Prometheus.BuildConfiguration.Image}
      />

      <TextField
        dispatch={dispatch}
        label="BuildConfiguration.Tag"
        type="BuildConfiguration.Tag"
        value={state.Prometheus.BuildConfiguration.Tag}
      />
    </>
  );
}
