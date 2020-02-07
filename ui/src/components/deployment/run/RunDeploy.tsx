import React, { useState } from 'react';
import { Button } from 'react-bootstrap';
import Steps from 'rc-steps';
import { PacmanLoader } from 'react-spinners';

function GetRandom() {
  return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
};

export default function RunDeploy() {
  const [steps, setSteps] = useState([
    {
      title: 'Initializing Deployment',
      description: ''
    },{
      title: 'Gathering Info',
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
    <>
      <Button className="mb-3" variant="outline-primary" onClick={addStep(GetRandom(), GetRandom())}>
        +
      </Button>
      <Steps current={steps.length - 1} direction="vertical" size="large">
        {steps.map((step, i) => {
          const icon = (
            <PacmanLoader size={15} color={'#007bff'} loading={true} />
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
    </>
  );
}
