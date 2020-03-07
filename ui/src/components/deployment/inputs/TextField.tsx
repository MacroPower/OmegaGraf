import React from 'react';
import { Form } from 'react-bootstrap';
import { Action, ActionTypes } from '../reducers/SettingsReducer';

export default function TextField(props: {
  dispatch: React.Dispatch<Action>;
  label: string;
  type: ActionTypes;
  value: any;
}) {

  let value = props.value || ''

  return (
    <>
      <Form.Group controlId={props.type.valueOf().toString()}>
        <Form.Label>{props.label}</Form.Label>
        <Form.Control
          required
          type="text"
          onChange={(e: any) =>
            props.dispatch({
              type: props.type,
              value: e.target.value
            })
          }
          value={value}
        />
        <Form.Control.Feedback type="invalid">Please enter a value!</Form.Control.Feedback>
      </Form.Group>
    </>
  );
}
