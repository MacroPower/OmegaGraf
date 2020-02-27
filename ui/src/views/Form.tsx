import React, { useState } from 'react';
import { UseGlobalSettings } from '../components/Global';
import { Redirect } from 'react-router-dom';
import { Form, Row, Col, Button } from 'react-bootstrap';
import { Settings } from '../components/settings/Settings';

interface State {
  state: Settings;
  title: string;
  description: string;
}

export default function FormView(props: React.PropsWithChildren<State>) {
  const [_, globalSettingsActions] = UseGlobalSettings();
  const { state, children, title, description } = props;
  const [toDeploy, redirect] = useState(false);
  return (
    <main role="main" className="container">
      <br />
      <h1 className="home-title text-center">{title}</h1>
      <h4 className="home-subtitle text-center">{description}</h4>
      <br />
      <hr />
      <br />

      <Row className="justify-content-md-center">
        <Form>
          {toDeploy && <Redirect to="/deploy" />}

          {children}

          <Row className="mt-2">
            <Col>
              <Button
                variant="primary"
                onClick={() => {
                  globalSettingsActions.setSettings(state);
                }}
              >
                Save
              </Button>
              <Button
                className="ml-2"
                variant="success"
                onClick={() => {
                  globalSettingsActions.setSettings(state);
                  redirect(true);
                }}
              >
                Deploy
              </Button>
            </Col>
          </Row>
        </Form>
      </Row>
    </main>
  );
}
