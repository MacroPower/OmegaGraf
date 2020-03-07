import { Session, defaultSession } from './Session';
import globalHook, { Store } from 'use-global-hook';
import React from 'react';
import { Settings } from './settings/Settings';
import { defaultSettings } from './settings/DefaultSettings';

type GlobalSessionActions = {
  setSession: (value: Session) => void;
};

const setSession = (
  store: Store<Session, GlobalSessionActions>,
  value: Session
) => {
  store.setState({ ...store.state, ...value });
};

export const UseGlobalSession = globalHook<Session, GlobalSessionActions>(
  React,
  defaultSession,
  { setSession: setSession }
);

//

export type GlobalSettingsActions = {
  setSettings: (value: Settings) => void;
};

const setSettings = (
  store: Store<Settings, GlobalSettingsActions>,
  value: Settings
) => {
  store.setState({ ...store.state, ...value });
};

export const UseGlobalSettings = globalHook<Settings, GlobalSettingsActions>(
  React,
  defaultSettings,
  { setSettings: setSettings }
);

//

export type Sim = {
  Active: boolean;
  Quantity: number;
};

export type GlobalSimActions = {
  setSim: (value: Sim) => void;
};

const setSim = (store: Store<Sim, GlobalSimActions>, value: Sim) => {
  store.setState({ ...store.state, ...value });
};

export const UseGlobalSim = globalHook<Sim, GlobalSimActions>(
  React,
  { Active: false, Quantity: 0 },
  { setSim: setSim }
);

//

export type Grafana = {
  Active: boolean;
};

export type GlobalGrafanaActions = {
  setGrafana: (value: Grafana) => void;
};

const setGrafana = (store: Store<Grafana, GlobalGrafanaActions>, value: Grafana) => {
  store.setState({ ...store.state, ...value });
};

export const UseGlobalGrafana = globalHook<Grafana, GlobalGrafanaActions>(
  React,
  { Active: true },
  { setGrafana: setGrafana }
);
