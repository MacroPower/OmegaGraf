import React, { useReducer, useEffect } from 'react';
import { SettingsReducer } from '../SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings } from '../../Global';
import FormView from '../../../views/Form';

export default function SimpleForm() {
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
      page="simple"
      pageName="Simple"
      title="Deploying Level 1"
      description="Please enter your preferences."
    >
      <AddSystem dispatch={dispatch} state={state} />
    </FormView>
  );
}
