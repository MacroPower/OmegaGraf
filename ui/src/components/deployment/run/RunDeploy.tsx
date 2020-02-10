import React, { useState, useEffect } from 'react';
import { Button } from 'react-bootstrap';
import Steps from 'rc-steps';
import { PacmanLoader } from 'react-spinners';
import { UseGlobalSettings, UseGlobalSession } from '../../Global';
import DeployRequest from './DeployRequest';
import PacmanGhost from '../../Ghost';
import { Settings } from '../../settings/Settings';
import Promise from 'thenfail';

type stepStatus = 'active' | 'error' | 'finish' | 'done';

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
      ...prev.slice(0, prev.length - 1),
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

    Promise.then(() =>
      deploySim(endpoint, state).then(() =>
        deployGrafana(endpoint, state).then(() =>
          deployTelegraf(endpoint, state).then(() =>
            deployPrometheus(endpoint, state).then(() => {
              const stepText = 'Cleaning up our mess...';
              addStep('Finishing up', stepText);
              setLastStep('Done', 'You can start using OmegaGraf!', 'done');
            })
          )
        )
      )
    );
  };

  const deployTelegraf = async (endpoint: string, state: Settings) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy Telegraf', stepText);
      await DeployRequest(endpoint, 'telegraf', state.Telegraf);
      setLastStep('Deploy Telegraf', stepText + 'Done!', 'finish');
    } catch (e) {
      setLastStep(
        'Deploy Telegraf',
        'Error creating container, please check server logs',
        'error'
      );
      const x = Promise.break;
    }
  };

  const deployPrometheus = async (endpoint: string, state: Settings) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy Prometheus', stepText);
      await DeployRequest(endpoint, 'prometheus', state.Prometheus);
      setLastStep('Deploy Prometheus', stepText + 'Done!', 'finish');
    } catch (e) {
      setLastStep(
        'Deploy Prometheus',
        'Error creating container, please check server logs',
        'error'
      );
      const x = Promise.break;
    }
  };

  const deployGrafana = async (endpoint: string, state: Settings) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy Grafana', stepText);
      await DeployRequest(endpoint, 'grafana', state.Grafana);
      setLastStep('Deploy Grafana', stepText + 'Done!', 'finish');
    } catch (e) {
      setLastStep(
        'Deploy Grafana',
        'Error creating container, please check server logs',
        'error'
      );
      const x = Promise.break;
    }
  };

  const deploySim = async (endpoint: string, state: Settings) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy VCSim', stepText);
      await DeployRequest(endpoint, 'telegraf/sim', state.VCSim);
      setLastStep('Deploy VCSim', stepText + 'Done!', 'finish');
    } catch (e) {
      setLastStep(
        'Deploy VCSim',
        'Error creating container, please check server logs',
        'error'
      );
      const x = Promise.break;
    }
  };

  const stepLength = () => {
    const l = steps.length - 1;
    console.log('Current step:' + l);
    return l;
  };

  return (
    <>
      {steps.length === 0 && (
        <Button className="mb-3" variant="outline-primary" onClick={startRun()}>
          Confirm
        </Button>
      )}
      {steps.length > 0 && (
        <Steps current={stepLength()} direction="vertical">
          {steps.map((step, i) => {
            const isError = step.status === 'error';

            const icon = !isError ? (
              step.status === 'done' ? (
                <i className="rcicon rcicon-check" />
              ) : (
                <PacmanLoader size={15} color={'#007bff'} loading={true} />
              )
            ) : (
              <PacmanGhost />
            );
            return (
              <Steps.Step
                key={i}
                {...step}
                {...(i === stepLength() && {
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
