import React from 'react';
import { Action } from '../SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { Settings } from '../../settings/Settings';

export default function SimpleForm(props: {
  dispatch: React.Dispatch<Action>;
  state: Settings;
}) {
  const { dispatch, state } = props;

  return (
    <>
      <AddSystem dispatch={dispatch} state={state} />
    </>
  );
}
