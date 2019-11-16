import { Session, defaultSession } from "./Session";
import globalHook, { Store } from "use-global-hook";
import React from "react";
import { Settings, defaultSettings } from "./Settings";

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

type GlobalSettingsActions = {
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