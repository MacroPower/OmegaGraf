import React, { useState } from 'react';
import { UseGlobalSession, UseGlobalSettings } from '../components/Global';
import { Container } from 'react-bootstrap';
import Steps from 'rc-steps';
import 'rc-steps/assets/index.css';
import 'rc-steps/assets/iconfont.css';

export default function Deploy() {
  const [globalSession, globalSessionActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

  return (
    <main role="main" className="container">
      <h2>Deploy</h2>
      <hr />
      <Container>
      <Steps current={1}>
        <Steps.Step title="first" />
        <Steps.Step title="second" />
        <Steps.Step title="third" />
        </Steps>
      </Container>
    </main>
  );
}
