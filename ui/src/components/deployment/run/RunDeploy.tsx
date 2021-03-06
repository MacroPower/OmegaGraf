import React, { useState } from 'react';
import Steps from 'rc-steps';
import { PacmanLoader } from 'react-spinners';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheckCircle } from '@fortawesome/free-solid-svg-icons';
import {
  UseGlobalSettings,
  UseGlobalSession,
  UseGlobalSim,
  UseGlobalGrafana,
} from '../../Global';
import DeployRequest from './DeployRequest';
import PacmanGhost from '../../../data/Ghost';
import { Settings } from '../../settings/Settings';
import Promise from 'thenfail';
import BigButton from '../../home/BigButton';
import Results from './Results';
import { Col, Row } from 'react-bootstrap';

type stepStatus = 'error' | 'finish' | 'process' | 'wait' | undefined;

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

  const [grafanaResults, showGrafanaResults] = useState(false);
  const [promResults, showPromResults] = useState(false);

  const grafanaPort = globalSettings.Grafana.BuildInput.Ports[3000];
  const prometheusPort = globalSettings.Prometheus.BuildInput.Ports[9090];

  const addStep = (title: string, description: string) => {
    setSteps((prev) => [
      ...prev,
      {
        title: title,
        description: description,
      },
    ]);
  };

  const setLastStep = (
    title: string,
    description: string,
    status?: stepStatus,
  ) => {
    setSteps((prev) => [
      ...prev.slice(0, prev.length - 1),
      {
        title: title,
        description: description,
        status: status,
      },
    ]);
  };

  const startRun = () => (e: any) => {
    addStep('Initializing Deployment', 'Initializing...');
    setLastStep(
      'Initializing Deployment',
      'Initializing...Initialization complete!',
      'wait',
    );

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
          setLastStep('Ready!', 'You can start using OmegaGraf.', 'finish');
        }),
    );
  };

  const breakPromise = () => Promise.break;

  const deployTelegraf = async (
    endpoint: string,
    apiKey: string,
    state: Settings,
  ) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy Telegraf', stepText);

      const conf = { ...state.Telegraf };

      if (globalSim.Active) {
        conf.Config[0].Data.Inputs.VSphere.forEach((x) => {
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
      setLastStep('Deploy Telegraf', stepText + 'Done!', 'wait');
    } catch (e) {
      setLastStep(
        'Deploy Telegraf',
        'Error creating container, please check server logs',
        'error',
      );
      breakPromise();
    }
  };

  const deployPrometheus = async (
    endpoint: string,
    apiKey: string,
    state: Settings,
  ) => {
    try {
      const stepText = 'Asking OmegaGraf to create the container...';
      addStep('Deploy Prometheus', stepText);
      await DeployRequest(endpoint, apiKey, 'prometheus', state.Prometheus);
      setLastStep('Deploy Prometheus', stepText + 'Done!', 'wait');
      showPromResults(true);
    } catch (e) {
      setLastStep(
        'Deploy Prometheus',
        'Error creating container, please check server logs',
        'error',
      );
      breakPromise();
    }
  };

  const deployGrafana = async (
    endpoint: string,
    apiKey: string,
    state: Settings,
  ) => {
    if (globalGrafana.Active) {
      try {
        const stepText = 'Asking OmegaGraf to create the container...';
        addStep('Deploy Grafana', stepText);
        await DeployRequest(endpoint, apiKey, 'grafana', state.Grafana);
        setLastStep('Deploy Grafana', stepText + 'Done!', 'wait');
      } catch (e) {
        setLastStep(
          'Deploy Grafana',
          'Error creating container, please check server logs',
          'error',
        );
        breakPromise();
      }
    }
  };

  const deploySim = async (
    endpoint: string,
    apiKey: string,
    state: Settings,
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
          setLastStep(stepTitle, stepText + 'Done!', 'wait');
        }
      } catch (e) {
        setLastStep(
          'Deploy VCSim',
          'Error creating container, please check server logs',
          'error',
        );
        breakPromise();
      }
    }
  };

  const deployGrafanaConfig = async (
    endpoint: string,
    apiKey: string,
    state: Settings,
  ) => {
    if (globalGrafana.Active) {
      try {
        const stepText = 'Asking OmegaGraf to update Grafana...';
        addStep('Deploy Grafana Config', stepText);

        const port = state.Grafana.BuildInput.Ports[3000].valueOf();
        let conf = { Port: port };

        await DeployRequest(endpoint, apiKey, 'grafana/datasource', conf);
        await DeployRequest(endpoint, apiKey, 'grafana/dashboards', conf);
        setLastStep('Deploy Grafana Config', stepText + 'Done!', 'wait');
        showGrafanaResults(true);
      } catch (e) {
        setLastStep(
          'Deploy Grafana Config',
          'Error configuring container, please check server logs',
          'error',
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
      <Row className="justify-content-md-center">
        <Col md={8}>
          {steps.length > 0 && (
            <Steps current={stepLength()} direction="vertical">
              {steps.map((step, i) => {
                const isError = step.status === 'error';

                const icon = !isError ? (
                  step.status === 'finish' ? (
                    <FontAwesomeIcon icon={faCheckCircle} />
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
                      icon: { ...icon },
                    })}
                  />
                );
              })}
            </Steps>
          )}
        </Col>
        <Col md={4}>
          {promResults && (
            <Results
              app="Prometheus"
              url={'http://' + (window.location.hostname || '+')}
              port={prometheusPort ? prometheusPort.valueOf().toString() : '0'}
            />
          )}
          {grafanaResults && (
            <Results
              app="Grafana"
              url={'http://' + (window.location.hostname || '+')}
              path="/d/BZOvRNrZz/welcome?orgId=1&refresh=5s"
              port={grafanaPort ? grafanaPort.valueOf().toString() : '0'}
              message="Login with admin/admin"
            />
          )}
        </Col>
      </Row>
    </>
  );
}
