import React from 'react';
import { Card } from 'react-bootstrap';
import { Action, ActionTypes } from '../reducers/SettingsReducer';
import { Settings } from '../../settings/Settings';
import TextField from './TextField';

export default function SetTags(props: {
  dispatch: React.Dispatch<Action>;
  state: Settings;
}) {
  const { dispatch, state } = props;

  return (
    <Card>
      <Card.Body>
        <Card.Title>Image Tags</Card.Title>
        <Card.Subtitle className="mb-2 text-muted">
          Edit the Docker image tags
        </Card.Subtitle>
        <Card.Text>
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
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
