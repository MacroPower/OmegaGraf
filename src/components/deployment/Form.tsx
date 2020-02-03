import React, { useState, useReducer, useEffect } from 'react';
import { UseGlobalSettings } from '../Global';
import { Button, Container, Row, Col } from 'react-bootstrap';
import { SettingsReducer } from './SettingsReducer';
import SimpleForm from './forms/SimpleForm';

export default function DeployForm() {
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

  const [state, dispatch] = useReducer(SettingsReducer, globalSettings);

  type formLevel = '1' | '2' | '3' | undefined;

  const [form, setForm] = useState<formLevel>(undefined);

  useEffect(() => {
    dispatch({
      type: 'reset',
      value: globalSettings
    });
  }, [globalSettings]);

  const submit = (e: any) => {
    e.preventDefault();

    globalSettingsActions.setSettings(state);
  };

  const renderSwitch = (param: formLevel) => {
    switch (param) {
      case '1':
        return <SimpleForm dispatch={dispatch} state={state} submit={submit} />;
      case '2':
        return null;
      case '3':
        return null;
      default:
        return null;
    }
  };

  const changeForm = (form: '1' | '2' | '3') => (e: any) => {
    setForm(form);
  };

  return (
    <>
      <Container>
        <Row>
          <Col>
            <Button variant="outline-primary" onClick={changeForm('1')}>
              I'm New!
            </Button>
          </Col>
          <Col>
            <Button variant="outline-primary" onClick={changeForm('2')}>
              Help me out
            </Button>
          </Col>
          <Col>
            <Button variant="outline-primary" onClick={changeForm('3')}>
              I know what I'm doing
            </Button>
          </Col>
        </Row>
      </Container>

      {renderSwitch(form)}
    </>
  );
}
