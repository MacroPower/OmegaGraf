import Cookies from "js-cookie";
import globalHook, { Store } from 'use-global-hook';
import React from "react";

export const defaultSession: Session = {
  endpoint: undefined,
  apiKey: undefined
};

type GlobalActions = {
  setValue: (value: Session) => void;
};

const setValue = (
  store: Store<Session, GlobalActions>,
  value: Session
) => {
  store.setState({ ...store.state, ...value });
};

const actions = {
  setValue
};

export const useGlobal = globalHook<Session, GlobalActions>(
  React,
  defaultSession,
  actions
);

export type Session = {
  endpoint: string | undefined;
  apiKey: string | undefined;
};

function IsSession(arg: any): arg is Session {
  return (
    arg.endpoint &&
    arg.apiKey
  );
}

export const setSessionCookie = (session: Session): void => {
  removeSessionCookie();
  Cookies.set("session", session, { expires: 1 });
};

export const removeSessionCookie = (): void => {
  Cookies.remove("session");
};

export const getSessionCookie = (): Session | null => {
  const sessionCookie = Cookies.get("session");

  if (sessionCookie !== undefined) {
    const session = JSON.parse(sessionCookie);
    if (IsSession(session)) {
      return session;
    }
  }

  return null;
};
