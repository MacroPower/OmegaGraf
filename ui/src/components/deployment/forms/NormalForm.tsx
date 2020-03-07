import React, { useReducer, useEffect, useState } from 'react';
import { SettingsReducer, ActionTypes } from '../reducers/SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings, UseGlobalGrafana } from '../../Global';
import TextField from '../inputs/TextField';
import FormView from '../../../views/Form';
import { Form } from 'react-bootstrap';

export default function NormalForm() {
  const [globalSettings] = UseGlobalSettings();
  const [state, dispatch] = useReducer(SettingsReducer, globalSettings);

  useEffect(() => {
    dispatch({
      type: ActionTypes.Reset,
      value: globalSettings
    });
  }, [globalSettings]);

  const [globalGrafana, globalGrafanaActions] = UseGlobalGrafana();
  const [grafana, setGrafana] = useState(globalGrafana.Active);

  return (
    <FormView
      state={state}
      page="normal"
      pageName="Normal"
      title="Deploying Level 2"
      description="Please enter your preferences."
    >
      <AddSystem dispatch={dispatch} state={state} />

      <br />

      <Form.Check
        custom
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

      <br />

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
        value={state.Prometheus.Config[0].Data.ScrapeConfigs[1]?.ScrapeInterval}
      />
    </FormView>
  );
}
