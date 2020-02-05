import React, { useState } from 'react';
import { UseGlobalSession, UseGlobalSettings } from '../components/Global';
import { Container, Button } from 'react-bootstrap';
import Steps from 'rc-steps';
import 'rc-steps/assets/index.css';
import 'rc-steps/assets/iconfont.css';
import { PacmanLoader } from 'react-spinners';

export default function Deploy() {
  const [globalSession, globalSessionActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

  const [steps, setSteps] = useState([
    {
      title: 'Gathering Info...',
      description: 'Fetching data from the OmegaGraf server.'
    }
  ]);

  const addStep = (title: string, description: string) => (e: any) => {
    const newSteps = [...steps];
    newSteps.push({
      title: title,
      description: description
    });
    setSteps(newSteps);
  };

  return (
    <main role="main" className="container">
      <h2>Deploy</h2>
      <hr />
      <Container>
        <Button variant="outline-primary" onClick={addStep('Test', 'Test')}>
          +
        </Button>
        <Steps current={steps.length - 1} direction="vertical" size="large">
          {steps.map((step, i) => {
            const icon = (
              <PacmanLoader
                size={15}
                color={'#007bff'}
                loading={true}
              />
            );

            return (
              <Steps.Step
                key={i}
                {...step}
                {...(i === steps.length - 1 && { icon: { ...icon }, style: { verticalAlign: '-.125em' } })}
              />
            );
          })}
        </Steps>
      </Container>
    </main>
  );
}
