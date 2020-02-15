import React, { useState, useEffect } from 'react';
import { Card, Button, Form, Row, Col } from 'react-bootstrap';
import { Action } from '../SettingsReducer';
import { Settings } from '../../settings/Settings';
import { UseGlobalSim } from '../../Global';

export default function AddSystem(props: {
  dispatch: React.Dispatch<Action>;
  state: Settings;
}) {
  const { dispatch, state } = props;

  const [globalSim, globalSimActions] = UseGlobalSim();

  const vSphere = state.Telegraf.Config[0].Data.Inputs.VSphere[0];

  const [systems, setSystems] = useState<string[]>(vSphere.VCenters || ['']);
  const [username, setUsername] = useState<string>(vSphere.Username || '');
  const [password, setPassword] = useState<string>(vSphere.Password || '');

  useEffect(() => {
    dispatch({
      type: 'vCenter',
      value: {
        systems: systems,
        username: username,
        password: password
      }
    });
  }, [dispatch, systems, username, password]);

  const [sim, setSim] = useState(globalSim.Active);

  return (
    <Card style={{ width: '24rem' }}>
      <Card.Body>
        <Card.Title>Add vCenter</Card.Title>
        <Card.Subtitle className="mb-2 text-muted">
          Enter your vCenter details
        </Card.Subtitle>
        <Card.Text>
          {systems.map((system: string, i: number) => {
            return (
              <Form.Group controlId={'formBasicSystem' + i}>
                <Form.Label>Address</Form.Label>
                <Row>
                  <Col md={i === 0 ? 12 : 10} className={i !== 0 ? 'pr-0' : ''}>
                    <Form.Control
                      className="inline"
                      type="text"
                      disabled={sim}
                      placeholder="vcenter.domain.local"
                      onChange={(e: any) => {
                        const ns = [...systems];
                        ns[i] = e.target.value;
                        setSystems([...ns]);
                      }}
                      value={system}
                    />
                  </Col>
                  {i > 0 && (
                    <Col md={2}>
                      <Button
                        className="inline"
                        variant="primary"
                        disabled={sim}
                        onClick={() => {
                          const ns = systems;
                          ns.splice(i, 1);
                          setSystems([...ns]);
                        }}
                      >
                        -
                      </Button>
                    </Col>
                  )}
                </Row>
                {i === systems.length - 1 &&
                  systems[systems.length - 1] !== '' && (
                    <Button
                      className="mt-3"
                      variant="primary"
                      disabled={sim}
                      onClick={() => {
                        const removedBlanks = systems.filter((v: string) => v);
                        setSystems([...removedBlanks, '']);
                      }}
                    >
                      +
                    </Button>
                  )}
              </Form.Group>
            );
          })}

          <Form.Group controlId="formBasicUsername">
            <Form.Label>Username</Form.Label>
            <Form.Control
              type="text"
              disabled={sim}
              placeholder="Username"
              onChange={(e: any) => {
                setUsername(e.target.value);
              }}
              value={username}
            />
          </Form.Group>

          <Form.Group controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              disabled={sim}
              placeholder="Password"
              onChange={(e: any) => {
                setPassword(e.target.value);
              }}
              value={password}
            />
          </Form.Group>
          <Form.Group controlId="formBasicCheckbox">
            <Form.Check
              type="checkbox"
              label="Use simulation"
              onChange={() => {
                const active = !sim;

                setSim(active);
                globalSimActions.setSim({
                  Active: active,
                  Quantity: 1
                });
              }}
              checked={sim}
            />
            {sim && (
              <Form.Control
                type="text"
                placeholder="Username"
                onChange={(e: any) => {
                  globalSimActions.setSim({
                    Active: true,
                    Quantity: e.target.value
                  });
                }}
                value={globalSim.Quantity.toString()}
              />
            )}
          </Form.Group>
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
