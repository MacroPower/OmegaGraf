import React, { useReducer, useEffect, useState } from 'react';
import { SettingsReducer } from '../SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings } from '../../Global';
import { Redirect } from 'react-router-dom';
import { Form, Row, Col, Button, Container } from 'react-bootstrap';
import TextField from '../inputs/TextField';

export default function NormalForm() {
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();
  const [state, dispatch] = useReducer(SettingsReducer, globalSettings);
  const [toDeploy, redirect] = useState(false);

  useEffect(() => {
    dispatch({
      type: 'reset',
      value: globalSettings
    });
  }, [globalSettings]);

  return (
    <Container>
      <Row className="justify-content-md-center">
        <Form>
          {toDeploy && <Redirect to="/deploy" />}

          <AddSystem dispatch={dispatch} state={state} />

          <TextField
            dispatch={dispatch}
            label="BuildConfiguration.Image"
            type="BuildConfiguration.Image"
            value={state.Prometheus.BuildConfiguration.Image}
          />

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
      <Row className="justify-content-md-center">
        <pre>{JSON.stringify(state, null, 1)}</pre>
      </Row>
    </Container>
  );
}
