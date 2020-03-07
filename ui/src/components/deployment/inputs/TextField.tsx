import React from 'react';
import { Form } from 'react-bootstrap';
import { Action, ActionTypes } from '../reducers/SettingsReducer';

type inputs = 'string' | 'number' | 'duration' | 'port';

const ports = /^(102[4-9])|(10[3-9][0-9])|(1[1-9][0-9][0-9])|([2-9][0-9][0-9][0-9])|([1-5][0-9][0-9][0-9][0-9])|(6[0-4][0-9][0-9][0-9])|(65[0-4][0-9][0-9])|(655[0-2][0-9])|(6553[0-5])$/;

const valid: { input: inputs; regex: RegExp; text: string }[] = [
  {
    input: 'string',
    regex: /.+/,
    text: 'Please enter a value.'
  },
  {
    input: 'number',
    regex: /[0-9]+/,
    text: 'Please enter a number.'
  },
  {
    input: 'duration',
    regex: /^([0-9]+)s$/,
    text: 'Please enter a time duration in seconds.'
  },
  {
    input: 'port',
    regex: ports,
    text: 'Please enter a number in range: 1024 - 65535.'
  }
];

export default function TextField(props: {
  dispatch: React.Dispatch<Action>;
  label: string;
  type: ActionTypes;
  value: string | number;
  input?: inputs;
  disabled?: boolean;
}) {
  const inputType = props.input || 'string';
  let value = props.value || '';

  return (
    <>
      {valid
        .filter(x => x.input === inputType)
        .map((x, _) => {
          const { regex, text } = x;

          let isValid = props.disabled || regex.test(value.toString());

          return (
            <Form.Group controlId={props.type.valueOf().toString()}>
              <Form.Label>{props.label}</Form.Label>
              <Form.Control
                required
                disabled={props.disabled || false}
                type="text"
                onChange={(e: any) => {
                  if (inputType === 'number' || inputType === 'port') {
                    const value: number = e.target.value;
                    if (value <= 65535) {
                      props.dispatch({
                        type: props.type,
                        value: value
                      })
                    }
                  }
                  else {
                    const value: string = e.target.value;
                    props.dispatch({
                      type: props.type,
                      value: value
                    })
                  }
                }}
                value={value.toString()}
                isValid={isValid}
                isInvalid={!isValid}
              />
              <Form.Control.Feedback type="invalid">
                {text}
              </Form.Control.Feedback>
            </Form.Group>
          );
        })}
    </>
  );
}
