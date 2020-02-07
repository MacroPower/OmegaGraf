import React, { useState, useEffect } from 'react';
import { Button, Form } from 'react-bootstrap';
import { setSessionCookie, Session } from '../components/Session';
import { Redirect } from 'react-router';
import {
  UseGlobalSession,
  UseGlobalSettings,
  getDefaults
} from '../components/Global';

export default function Login() {
  const [globalState, globalActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();
  const [key, setKey] = useState('');
  const [toHome, redirect] = useState(false);

  useEffect(() => {
    if (globalState.apiKey !== undefined) {
      redirect(true);
    }
  }, [globalState.apiKey]);

  const submit = (e: any) => {
    e.preventDefault();

    //eslint-disable-next-line
    const host: string = location.host;

    const session: Session = {
      endpoint: host,
      apiKey: key
    };

    // TODO: VALIDATE API KEY HERE

    setSessionCookie(session);
    globalActions.setSession(session);

    getDefaults(session, globalSettingsActions);

    redirect(true);
  };

  return (
    <main role="main" className="container">
      {toHome && <Redirect to="/" />}
      <h2>Login</h2>
      <Form onSubmit={submit}>
        <Form.Group controlId="formKey">
          <Form.Label>OmegaKey</Form.Label>
          <Form.Control
            value={key}
            onChange={(e: any) => setKey(e.target.value)}
            type="text"
            placeholder="Unique Key"
          />
        </Form.Group>
        <Button variant="primary" type="submit">
          Submit
        </Button>
      </Form>
    </main>
  );
}
