import React, { useState } from 'react';
import { Card, Button, Form, Row, Col } from 'react-bootstrap';
import { Action } from '../SettingsReducer';
import { Settings } from '../../settings/Settings';

export default function AddSystem(props: {
  dispatch: React.Dispatch<Action>;
  state: Settings;
}) {
  const { dispatch, state } = props;

  const vSphere = state.Telegraf.Config[0].Data.Inputs.VSphere[0];

  const vCenters = vSphere.VCenters || [''];
  const username = vSphere.Username || '';
  const password = vSphere.Password || '';

  const [systems, setSystems] = useState<string[]>(vCenters);

  const [sim, setSim] = useState(false);

  return (
    <Card style={{ width: '24rem' }}>
      <Card.Body>
        <Card.Title>Add vCenter</Card.Title>
        <Card.Subtitle className="mb-2 text-muted">
          Enter your vCenter details
        </Card.Subtitle>
        <Card.Text>
          {systems
            .filter(v => v !== undefined)
            .map((system: string, i: number) => {
              return (
                <Form.Group controlId={'formBasicSystem' + i}>
                  <Form.Label>Address</Form.Label>
                  <Row>
                    <Col
                      md={i === 0 ? 12 : 10}
                      className={i !== 0 ? 'pr-0' : ''}
                    >
                      <Form.Control
                        className="inline"
                        type="text"
                        disabled={sim}
                        placeholder="vcenter.domain.local"
                        onChange={(e: any) =>
                          dispatch({
                            type: 'Address',
                            index: i,
                            current: systems,
                            value: e.target.value
                          })
                        }
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
                            dispatch({
                              type: 'Remove',
                              index: i,
                              current: systems,
                              value: ''
                            });

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
                          dispatch({
                            type: 'Add',
                            index: i,
                            current: systems,
                            value: ''
                          });
                          const removedBlanks = systems.filter(
                            (v: string) => v
                          );
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
              onChange={(e: any) =>
                dispatch({
                  type: 'Username',
                  value: e.target.value
                })
              }
            />
          </Form.Group>

          <Form.Group controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              disabled={sim}
              placeholder="Password"
              onChange={(e: any) =>
                dispatch({
                  type: 'Password',
                  value: e.target.value
                })
              }
            />
          </Form.Group>
          <Form.Group controlId="formBasicCheckbox">
            <Form.Check
              type="checkbox"
              label="Use simulation"
              onChange={() => {
                setSim(!sim);
              }}
            />
          </Form.Group>
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
