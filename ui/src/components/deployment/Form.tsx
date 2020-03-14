import React, { useState, useEffect } from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import { Route, Link } from 'react-router-dom';
import OptionCard from '../OptionCard';
import BigButton from '../BigButton';
import setDefaults from '../settings/SetDefaults';
import {
  UseGlobalSession,
  UseGlobalSettings,
  UseGlobalGrafana,
  UseGlobalSim
} from '../Global';

function RoutedLink(props: { to: string; disabled: boolean }) {
  const data = {
    label: '',
    path: props.to,
    exact: false,
    children: () => (
      <Link to={props.to} className="text-decoration-none">
        <BigButton disabled={props.disabled}>Get Started</BigButton>
      </Link>
    )
  };

  return <Route {...data} />;
}

export default function DeployForm() {
  const [direct, setDirect] = useState('');

  const globalState = UseGlobalSession()[0];
  const globalSettingsActions = UseGlobalSettings()[1];
  const globalGrafanaActions = UseGlobalGrafana()[1];
  const globalSimActions = UseGlobalSim()[1];

  useEffect(() => {
    setDefaults(
      globalState,
      globalSettingsActions,
      globalGrafanaActions,
      globalSimActions
    );
  }, [
    globalState,
    globalSettingsActions,
    globalGrafanaActions,
    globalSimActions
  ]);

  return (
    <Container>
      <Row className="mb-4 justify-content-md-center">
        <Col md={4}>
          <Button
            variant="success"
            className="card-button"
            onClick={() => setDirect('simple')}
          >
            <OptionCard clicked={direct === 'simple'} phase="1" />
          </Button>
        </Col>
        <Col md={4}>
          <Button className="card-button" onClick={() => setDirect('normal')}>
            <OptionCard clicked={direct === 'normal'} phase="2" />
          </Button>
        </Col>
        <Col md={4}>
          <Button
            variant="danger"
            className="card-button"
            onClick={() => setDirect('advanced')}
          >
            <OptionCard clicked={direct === 'advanced'} phase="3" />
          </Button>
        </Col>
      </Row>
      <br />
      <Row className="mt-4 justify-content-md-center">
        <RoutedLink to={'/form/' + direct} disabled={direct === ''} />
      </Row>
    </Container>
  );
}
