import React, { useState, useEffect } from 'react';
import { Button, Form, Alert } from 'react-bootstrap';
import { setSessionCookie, Session } from '../components/Session';
import { Redirect } from 'react-router';
import { UseGlobalSession } from '../components/Global';

export default function Login() {
  const [globalState, globalActions] = UseGlobalSession();
  const [key, setKey] = useState('');
  const [toHome, redirect] = useState(false);
  const [error, setError] = useState<string | undefined>(undefined);

  useEffect(() => {
    if (globalState.apiKey !== undefined) {
      redirect(true);
    }
  }, [globalState.apiKey]);

  const submit = (e: any) => {
    e.preventDefault();

    //eslint-disable-next-line
    const host: string = location.protocol + '//' + location.host;

    fetch(host + '/auth', {
      method: 'GET',
      headers: {
        Authorization: key
      }
    })
      .then(r => {
        if (r.ok != true) {
          throw new Error('API returned status ' + r.status.toString());
        }
        return r;
      })
      .then(r => r.json())
      .then(r => {
        if (r.Authenticated) {
          return r;
        } else {
          throw new Error('Unauthorized');
        }
      })
      .then(() => {
        const session: Session = {
          endpoint: host,
          apiKey: key
        };

        setSessionCookie(session);
        globalActions.setSession(session);

        redirect(true);
      })
      .catch((e: Error) => {
        if (e.name === 'SyntaxError') {
          setError('Error connecting to the OmegaGraf server.');
        } else {
          setError(e.message);
        }
      });
  };

  return (
    <main role="main" className="container">
      {toHome && <Redirect to="/" />}
      <h1>Login</h1>
      <hr />
      <Form onSubmit={submit}>
        <Form.Group controlId="formKey">
          <Form.Label>
            Please enter the <b>OmegaGraf Secure Key</b> that was printed to
            your console
          </Form.Label>
          <Form.Control
            value={key}
            onChange={(e: any) => setKey(e.target.value)}
            type="text"
            placeholder="OmegaGraf Secure Key"
          />
        </Form.Group>
        {error && <Alert variant="danger">{error}</Alert>}
        <Button variant="primary" type="submit" disabled={key.length === 0}>
          Login
        </Button>
      </Form>
    </main>
  );
}
