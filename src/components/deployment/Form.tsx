import React, { useState, useReducer, useEffect, useLayoutEffect } from 'react';
import { UseGlobalSession, UseGlobalSettings } from '../Global';
import { Form, Button } from 'react-bootstrap';
import { Action, SettingsReducer } from './SettingsReducer';
import TextField from './TextField';

export default function DeployForm() {
  const [globalSession, globalSessionActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

  const [state, dispatch] = useReducer(SettingsReducer, globalSettings);

  useEffect(() => {
    dispatch({
      type: 'reset',
      value: globalSettings
    });
  }, [globalSettings]);

  const submit = (e: any) => {
    e.preventDefault();

    globalSettingsActions.setSettings(state);
  };

  return (
    <>
      <Form onSubmit={submit}>
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

        <Button variant="primary" type="submit">
          Submit
        </Button>
      </Form>
      <pre>{JSON.stringify(state, null, 1)}</pre>
    </>
  );
}
