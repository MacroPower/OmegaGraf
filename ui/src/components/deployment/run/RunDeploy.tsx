import React, { useState } from 'react';
import Steps from 'rc-steps';
import { PacmanLoader } from 'react-spinners';
import {
  UseGlobalSettings,
  UseGlobalSession,
  UseGlobalSim,
  UseGlobalGrafana
} from '../../Global';
import DeployRequest from './DeployRequest';
import PacmanGhost from '../../../data/Ghost';
import { Settings } from '../../settings/Settings';
import Promise from 'thenfail';
import BigButton from '../../BigButton';

type stepStatus = 'active' | 'error' | 'finish' | 'done';

type step = {
  title: string;
  description: string;
  status?: stepStatus;
}[];

export default function RunDeploy() {
  const [globalState] = UseGlobalSession();
  const [globalSettings] = UseGlobalSettings();
  const [globalSim] = UseGlobalSim();
  const [globalGrafana] = UseGlobalGrafana();

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
    const apiKey = globalState.apiKey || '';

    const state = { ...globalSettings };

    Promise.then(() =>
      deploySim(endpoint, apiKey, state)
        .then(() => deployGrafana(endpoint, apiKey, state))
        .then(() => deployTelegraf(endpoint, apiKey, state))
        .then(() => deployPrometheus(endpoint, apiKey, state))
        .then(() => deployGrafanaConfig(endpoint, apiKey, state))
        .then(() => {
          const stepText = 'Cleaning up our mess...';
          addStep('Finishing up', stepText);
          setLastStep('Done', 'You can start using OmegaGraf!', 'done');
        })
    );
  };

  const breakPromise = () => Promise.break;

  const deployTelegraf = async (
    endpoint: string,
    apiKey: string,
    state: Settings
  ) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy Telegraf', stepText);

      const conf = { ...state.Telegraf };

      if (globalSim.Active) {
        conf.Config[0].Data.Inputs.VSphere.forEach(x => {
          let vcs: string[] = [];

          for (let i = 0; i < globalSim.Quantity; i++) {
            const iq = i + 1;
            vcs.push('https://og-vcsim' + iq.toString() + ':8989/sdk');
          }

          x.VCenters = vcs;

          x.Username = 'user';
          x.Password = 'pass';
        });
      }

      await DeployRequest(endpoint, apiKey, 'telegraf', conf);
      setLastStep('Deploy Telegraf', stepText + 'Done!', 'finish');
    } catch (e) {
      setLastStep(
        'Deploy Telegraf',
        'Error creating container, please check server logs',
        'error'
      );
      breakPromise();
    }
  };

  const deployPrometheus = async (
    endpoint: string,
    apiKey: string,
    state: Settings
  ) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy Prometheus', stepText);
      await DeployRequest(endpoint, apiKey, 'prometheus', state.Prometheus);
      setLastStep('Deploy Prometheus', stepText + 'Done!', 'finish');
    } catch (e) {
      setLastStep(
        'Deploy Prometheus',
        'Error creating container, please check server logs',
        'error'
      );
      breakPromise();
    }
  };

  const deployGrafana = async (
    endpoint: string,
    apiKey: string,
    state: Settings
  ) => {
    if (globalGrafana.Active) {
      try {
        const stepText = 'Asking OmegaGraf to create the container...';
        addStep('Deploy Grafana', stepText);
        await DeployRequest(endpoint, apiKey, 'grafana', state.Grafana);
        setLastStep('Deploy Grafana', stepText + 'Done!', 'finish');
      } catch (e) {
        setLastStep(
          'Deploy Grafana',
          'Error creating container, please check server logs',
          'error'
        );
        breakPromise();
      }
    }
  };

  const deploySim = async (
    endpoint: string,
    apiKey: string,
    state: Settings
  ) => {
    if (globalSim.Active) {
      try {
        const stepText = 'Asking OmegaGraf to create the container...';

        for (let i = 0; i < globalSim.Quantity; i++) {
          const iq = i + 1;
          const stepTitle =
            globalSim.Quantity > 1 ? 'Deploy VCSim #' + iq : 'Deploy VCSim';
          addStep(stepTitle, stepText);

          let conf = { ...state.VCSim };
          conf.BuildInput.Name = 'vcsim' + iq;

          await DeployRequest(endpoint, apiKey, 'telegraf/sim', conf);
          setLastStep(stepTitle, stepText + 'Done!', 'finish');
        }
      } catch (e) {
        setLastStep(
          'Deploy VCSim',
          'Error creating container, please check server logs',
          'error'
        );
        breakPromise();
      }
    }
  };

  const deployGrafanaConfig = async (
    endpoint: string,
    apiKey: string,
    state: Settings
  ) => {
    if (globalGrafana.Active) {
      try {
        const stepText = 'Asking OmegaGraf to update Grafana...';
        addStep('Deploy Grafana Config', stepText);

        const port = state.Grafana.BuildInput.Ports[3000].valueOf();
        let conf = { Port: port };

        await DeployRequest(endpoint, apiKey, 'grafana/datasource', conf);
        await DeployRequest(endpoint, apiKey, 'grafana/dashboards', conf);
        setLastStep('Deploy Grafana Config', stepText + 'Done!', 'finish');
      } catch (e) {
        setLastStep(
          'Deploy Grafana Config',
          'Error configuring container, please check server logs',
          'error'
        );
        breakPromise();
      }
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
        <div className="text-center">
          <h4>Are you sure you want to deploy?</h4>
          <br />
          <BigButton onClick={startRun()}>Confirm</BigButton>
        </div>
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
