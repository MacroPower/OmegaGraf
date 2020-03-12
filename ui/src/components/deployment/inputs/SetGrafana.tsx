import React, { useState } from 'react';
import { Card, Form } from 'react-bootstrap';
import { Action, ActionTypes } from '../reducers/SettingsReducer';
import { Settings } from '../../settings/Settings';
import TextField from './TextField';
import { UseGlobalGrafana } from '../../Global';

export default function SetGrafana(props: {
  dispatch: React.Dispatch<Action>;
  state: Settings;
}) {
  const { dispatch, state } = props;

  const [globalGrafana, globalGrafanaActions] = UseGlobalGrafana();
  const [grafana, setGrafana] = useState(globalGrafana.Active);
  const port = state.Grafana.BuildInput.Ports[3000];

  return (
    <Card>
      <Card.Body>
        <Card.Title>Grafana</Card.Title>
        <Card.Subtitle className="mb-2 text-muted">
          Change the Grafana deployment
        </Card.Subtitle>
        <Card.Text>
          <Form.Check
            custom
            className="mt-1 mb-1"
            id="custom-checkbox"
            type="checkbox"
            label="Deploy Grafana Instance"
            onChange={() => {
              const active = !grafana;
              setGrafana(active);

              globalGrafanaActions.setGrafana({
                Active: active
              });
            }}
            checked={grafana}
          />
          <TextField
            disabled={!grafana}
            dispatch={dispatch}
            label="Grafana Port Number"
            type={ActionTypes.GrafanaBuildConfigurationPort}
            input="port"
            value={port ? port.valueOf().toString() : ''}
          />
          <TextField
            disabled={!grafana}
            dispatch={dispatch}
            label="Grafana Tag"
            type={ActionTypes.GrafanaBuildConfigurationTag}
            value={state.Grafana.BuildInput.Tag}
          />
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
