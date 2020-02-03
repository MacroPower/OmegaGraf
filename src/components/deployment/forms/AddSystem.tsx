import React, { useState } from 'react';
import { Card, Button, Form } from 'react-bootstrap';
import { Action } from '../SettingsReducer';
import { Settings } from '../../settings/Settings';

export default function AddSystem(props: {
    dispatch: React.Dispatch<Action>;
    state: any;
}) {
  const { dispatch, state } = props;

  const systemState: Settings = state;
  const vCenters = systemState.Telegraf.Config[0].Data.Inputs.VSphere;

  const [systems, setSystems] = useState<string[]>(vCenters[0].VCenters);

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
              <Form.Group controlId={"formBasicSystem" + i}>
                <Form.Label>Address</Form.Label>
                <Form.Control
                  type="text"
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
                {i > 0 && (
                  <Button
                    variant="primary"
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
                )}
                {i === systems.length - 1 &&
                  systems[systems.length - 1] !== '' && (
                    <Button
                      variant="primary"
                      onClick={() => {
                        dispatch({
                          type: 'Add',
                          index: i,
                          current: systems,
                          value: ''
                        });
                        setSystems([...systems, '']);
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
            <Form.Control type="text" placeholder="Username" />
          </Form.Group>

          <Form.Group controlId="formBasicPassword">
            <Form.Label>Password</Form.Label>
            <Form.Control type="password" placeholder="Password" />
          </Form.Group>
          <Form.Group controlId="formBasicCheckbox">
            <Form.Check type="checkbox" label="Check me out" />
          </Form.Group>
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
