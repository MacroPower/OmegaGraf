import React, { useReducer, useEffect } from 'react';
import { SettingsReducer, ActionTypes } from '../reducers/SettingsReducer';
import AddSystem from '../inputs/AddSystem';
import { UseGlobalSettings } from '../../Global';
import FormView from '../../../views/Form';
import { Col, Row } from 'react-bootstrap';
import SetTags from '../inputs/SetTags';
import SetGrafana from '../inputs/SetGrafana';
import SetScrape from '../inputs/SetScrape';

export default function AdvancedForm() {
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
      page="advanced"
      pageName="Advanced"
      title="Deploying Level 3"
      description="Please enter your preferences."
    >
      <Row className="justify-content-md-center">
        <Col lg={6} md={12} sm={12}>
          <br />
          <AddSystem dispatch={dispatch} state={state} />
          <br />
          <SetTags dispatch={dispatch} state={state} />
        </Col>
        <Col lg={6} md={12} sm={12}>
          <br />
          <SetGrafana dispatch={dispatch} state={state} />
          <br />
          <SetScrape dispatch={dispatch} state={state} />
        </Col>
      </Row>
    </FormView>
  );
}
