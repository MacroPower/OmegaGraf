import React from 'react';
import { Action } from '../SettingsReducer';
import AddSystem from './AddSystem';

export default function SimpleForm(props: {
  dispatch: React.Dispatch<Action>;
  state: any;
}) {
  const { dispatch, state } = props;

  return (
    <>
      <AddSystem dispatch={dispatch} state={state} />
    </>
  );
}
