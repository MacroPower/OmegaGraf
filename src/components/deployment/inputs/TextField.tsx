import React from 'react';
import { Form } from 'react-bootstrap';
import { Action } from '../SettingsReducer';

export default function TextField(props: {
  dispatch: React.Dispatch<Action>;
  label: string;
  type: string;
  value: any;
}) {
  return (
    <>
      <Form.Group controlId={props.type}>
        <Form.Label>{props.label}</Form.Label>
        <Form.Control
          type="text"
          onChange={(e: any) =>
            props.dispatch({
              type: props.type,
              value: e.target.value
            })
          }
          value={props.value}
        />
      </Form.Group>
    </>
  );
}
