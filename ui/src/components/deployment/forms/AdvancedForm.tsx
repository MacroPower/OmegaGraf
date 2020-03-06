import React, { useReducer, useEffect, useState } from 'react';
import { SettingsReducer, ActionTypes } from '../reducers/SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings, UseGlobalGrafana } from '../../Global';
import TextField from '../inputs/TextField';
import FormView from '../../../views/Form';
import { Form } from 'react-bootstrap';

export default function AdvancedForm() {
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

  const port = state.Grafana.BuildInput.Ports[3000];

  return (
    <FormView
      state={state}
      page="advanced"
      pageName="Advanced"
      title="Deploying Level 3"
      description="Please enter your preferences."
    >
      <AddSystem dispatch={dispatch} state={state} />

      <br />

      <Form.Check
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
        value={state.Prometheus.Config[0].Data.Global.ScrapeInterval}
      />

      <TextField
        dispatch={dispatch}
        label="Prometheus vCenter Scrape Interval"
        type={ActionTypes.PrometheusConfigDataScrapeIntervalLong}
        value={state.Prometheus.Config[0].Data.ScrapeConfigs[1]?.ScrapeInterval}
      />

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

      {grafana && (
        <TextField
          dispatch={dispatch}
          label="Grafana Tag"
          type={ActionTypes.GrafanaBuildConfigurationTag}
          value={state.Grafana.BuildInput.Tag}
        />
      )}

      {grafana && (
        <TextField
          dispatch={dispatch}
          label="Grafana Port Number"
          type={ActionTypes.GrafanaBuildConfigurationPort}
          value={port ? port.valueOf().toString() : '0'}
        />
      )}
    </FormView>
  );
}
