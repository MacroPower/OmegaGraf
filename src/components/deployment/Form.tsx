import React, { useState, useReducer } from "react";
import { UseGlobalSession, UseGlobalSettings } from "../Global";
import { Form, Button } from "react-bootstrap";
import { Settings } from "../Settings";

type Action = {
  type: string;
  value: string;
};

function reducer(state: Settings, action: Action) {
  switch (action.type) {
    case "BuildConfiguration.Image":
      return {
        ...state,
        Prometheus: {
          BuildConfiguration: {
            ...state.Prometheus.BuildConfiguration,
            Image: action.value
          },
          Config: state.Prometheus.Config
        }
      };
    case "BuildConfiguration.Tag":
      return {
        ...state,
        Prometheus: {
          BuildConfiguration: {
            ...state.Prometheus.BuildConfiguration,
            Tag: action.value
          },
          Config: state.Prometheus.Config
        }
      };
    default:
      throw new Error();
  }
}

export default function DeployForm() {
  const [globalSession, globalSessionActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

  const [state, dispatch] = useReducer(reducer, globalSettings);

  const submit = (e: any) => {
    e.preventDefault();

    globalSettingsActions.setSettings(state);
  };

  return (
    <>
      <Form onSubmit={submit}>
        <Form.Group controlId="formField1">
          <Form.Label>Test1</Form.Label>
          <Form.Control
            type="text"
            onChange={(e: any) =>
              dispatch({
                type: "BuildConfiguration.Image",
                value: e.target.value
              })
            }
            value={state.Prometheus.BuildConfiguration.Image}
          />
        </Form.Group>

        <Form.Group controlId="formField2">
          <Form.Label>Test2</Form.Label>
          <Form.Control
            type="text"
            onChange={(e: any) =>
              dispatch({
                type: "BuildConfiguration.Tag",
                value: e.target.value
              })
            }
            value={state.Prometheus.BuildConfiguration.Tag}
          />
        </Form.Group>

        <Button variant="primary" type="submit">
          Submit
        </Button>
      </Form>
      <pre>{JSON.stringify(state, null, 1)}</pre>
    </>
  );
}
