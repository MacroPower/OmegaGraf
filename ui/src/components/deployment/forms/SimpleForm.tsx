import React, { useReducer, useEffect } from 'react';
import { SettingsReducer, ActionTypes } from '../reducers/SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings } from '../../Global';
import FormView from '../../../views/Form';
import { Row, Col } from 'react-bootstrap';

export default function SimpleForm() {
  const [globalSettings] = UseGlobalSettings();
  const [state, dispatch] = useReducer(SettingsReducer, globalSettings);

  useEffect(() => {
    dispatch({
      type: ActionTypes.Reset,
      value: globalSettings
    });
  }, [globalSettings]);

  return (
    <FormView
      state={state}
      page="simple"
      pageName="Simple"
      title="Deploying Level 1"
      description="Please enter your preferences."
    >
      <Row className="justify-content-md-center">
        <Col lg={6} md={12}>
          <br />
          <AddSystem dispatch={dispatch} state={state} />
        </Col>
      </Row>
    </FormView>
  );
}
