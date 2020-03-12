import React from 'react';
import { Card } from 'react-bootstrap';
import { Action, ActionTypes } from '../reducers/SettingsReducer';
import { Settings } from '../../settings/Settings';
import TextField from './TextField';

export default function SetScrape(props: {
  dispatch: React.Dispatch<Action>;
  state: Settings;
}) {
  const { dispatch, state } = props;

  return (
    <Card>
      <Card.Body>
        <Card.Title>Scrape Settings</Card.Title>
        <Card.Subtitle className="mb-2 text-muted">
          Adjust the scrape intervals
        </Card.Subtitle>
        <Card.Text>
          <TextField
            dispatch={dispatch}
            label="Prometheus Global Scrape Interval"
            type={ActionTypes.PrometheusConfigDataScrapeIntervalShort}
            input="duration"
            value={state.Prometheus.Config[0].Data.Global.ScrapeInterval}
          />

          <TextField
            dispatch={dispatch}
            label="Prometheus vCenter Scrape Interval"
            type={ActionTypes.PrometheusConfigDataScrapeIntervalLong}
            input="duration"
            value={
              state.Prometheus.Config[0].Data.ScrapeConfigs[1]?.ScrapeInterval
            }
          />
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
