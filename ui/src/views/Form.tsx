import React, { useState } from 'react';
import { UseGlobalSettings } from '../components/Global';
import { Redirect, Link } from 'react-router-dom';
import { Form, Row, Col, Button, Breadcrumb } from 'react-bootstrap';
import { Settings } from '../components/settings/Settings';

interface State {
  state: Settings;
  page: string;
  pageName: string;
  title: string;
  description: string;
}

export default function FormView(props: React.PropsWithChildren<State>) {
  const globalSettingsActions = UseGlobalSettings()[1];
  const { state, children, title, description, page } = props;
  const [toDeploy, redirect] = useState(false);
  const [validated, setValidated] = useState(false);

  const handleSubmit = (event: any) => {
    const form = event.currentTarget;
    if (form.checkValidity() === false) {
      event.preventDefault();
      event.stopPropagation();
    } else {
      globalSettingsActions.setSettings(state);
      redirect(true);
    }

    setValidated(true);
  };

  return (
    <main role="main" className="container">
      <Breadcrumb>
        <Breadcrumb.Item>
          <Link to="/">Home</Link>
        </Breadcrumb.Item>
        <Breadcrumb.Item active>Form</Breadcrumb.Item>
      </Breadcrumb>
      <br />
      <h1 className="home-title text-center">{title}</h1>
      <h4 className="home-subtitle text-center">{description}</h4>
      <br />
      <hr />
      <Form noValidate validated={validated} onSubmit={handleSubmit}>
        {toDeploy && <Redirect to={'/deploy?ref=' + page} push />}

        {children}

        <Row className="justify-content-md-center mt-4">
          <Col lg={4} md={12}>
            <Button variant="success" type="submit" block>
              Deploy
            </Button>
          </Col>
        </Row>
      </Form>
      <br />
    </main>
  );
}
