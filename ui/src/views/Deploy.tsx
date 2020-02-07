import React from 'react';
import { UseGlobalSession, UseGlobalSettings } from '../components/Global';
import { Container } from 'react-bootstrap';
import 'rc-steps/assets/index.css';
import 'rc-steps/assets/iconfont.css';
import RunDeploy from '../components/deployment/run/RunDeploy';

export default function Deploy() {
  const [globalSession, globalSessionActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

  return (
    <main role="main" className="container">
      <h2>Deploy</h2>
      <hr />
      <Container>
        <RunDeploy />
      </Container>
    </main>
  );
}
