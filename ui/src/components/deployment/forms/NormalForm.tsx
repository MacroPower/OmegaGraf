import React from 'react';
import { Action } from '../SettingsReducer';
import TextField from '../inputs/TextField';
import AddSystem from '../inputs/AddSystem';
import { Settings } from '../../settings/Settings';

export default function NormalForm(props: {
  dispatch: React.Dispatch<Action>;
  state: Settings;
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
    </>
  );
}
