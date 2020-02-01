import React from 'react';
import { Form } from 'react-bootstrap';
import { Action } from '../SettingsReducer';
import TextField from '../inputs/TextField';
import { Button } from 'react-bootstrap';

export default function SimpleForm(props: {
  dispatch: React.Dispatch<Action>;
  submit: any;
  state: any;
}) {
  const { dispatch, submit, state } = props;

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
