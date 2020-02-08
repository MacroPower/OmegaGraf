import React, { useState } from 'react';
import { Button } from 'react-bootstrap';
import Steps from 'rc-steps';
import { PacmanLoader } from 'react-spinners';
import { UseGlobalSettings, UseGlobalSession } from '../../Global';
import DeployRequest from './DeployRequest';

type stepStatus = 'working' | 'error';

type step = {
  title: string;
  description: string;
  status?: stepStatus;
}[];

export default function RunDeploy() {
  const [globalState] = UseGlobalSession();
  const [globalSettings] = UseGlobalSettings();

  const [steps, setSteps] = useState<step>([]);

  const addStep = (title: string, description: string) => {
    setSteps(prev => [
      ...prev,
      {
        title: title,
        description: description
      }
    ]);
  };

  const setLastStep = (
    title: string,
    description: string,
    status?: stepStatus
  ) => {
    setSteps(prev => [
      ...prev.slice(0, prev.length),
      {
        title: title,
        description: description,
        status: status
      }
    ]);
  };

  const startRun = () => (e: any) => {
    addStep('Initializing Deployment', 'Please wait');

    const endpoint = globalState.endpoint || '';

    const state = { ...globalSettings };

    DeployRequest(endpoint, 'telegraf', state.Telegraf.toString())
      .then(() => addStep('Deploy Telegraf', 'Asking OmegaGraf to create the container'))
      .catch(() => setLastStep('Deploy Telegraf', 'Error Exception', 'error'));
  };

  return (
    <>
      {steps.length === 0 && (
        <Button className="mb-3" variant="outline-primary" onClick={startRun()}>
          Confirm
        </Button>
      )}
      {steps.length > 0 && (
        <Steps current={steps.length - 1} direction="vertical" size="large">
          {steps.map((step, i) => {
            const isError = step.status === 'error';
            const iconColor = isError ? '#f50' : '#007bff';
            const icon = (
              <PacmanLoader size={15} color={iconColor} loading={true} />
            );
            return (
              <Steps.Step
                key={i}
                {...step}
                {...(i === steps.length - 1 && {
                  icon: { ...icon }
                })}
              />
            );
          })}
        </Steps>
      )}
    </>
  );
}
