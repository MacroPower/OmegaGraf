import React, { useEffect } from 'react';
import './styles/App.css';
import { BrowserRouter as Router } from 'react-router-dom';
import { AppliedRoutes } from './components/Routes';
import Footer from './components/Footer';
import HeaderNav from './components/Header';
import Navbar from 'react-bootstrap/Navbar';
import { getSessionCookie, removeSessionCookie } from './components/Session';
import {
  UseGlobalSession,
  getDefaults,
  UseGlobalSettings
} from './components/Global';
import Logo from './data/Logo';

export default function App() {
  const [globalState, globalActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

  useEffect(() => {
    const session = getSessionCookie();

    console.log(session);

    if (session !== null) {
      fetch(session.endpoint + '/auth', {
        method: 'GET',
        headers: {
          Authorization: '' + session.apiKey
        }
      })
        .then(r => {
          if (r.ok != true) {
            throw new Error('API returned status ' + r.status.toString());
          }
          return r;
        })
        .then(r => r.json())
        .then(r => {
          if (r.Authenticated) {
            return r;
          } else {
            throw new Error('Unauthorized');
          }
        })
        .then(() => {
          globalActions.setSession(session);
          getDefaults(session, globalSettingsActions);
        })
        .catch((e: Error) => {
          if (e.name === 'SyntaxError') {
            console.log('Error connecting to the OmegaGraf server.');
          } else {
            console.log(e);
          }

          removeSessionCookie();

          globalActions.setSession({
            endpoint: undefined,
            apiKey: undefined
          });
        });
    }
  }, [globalActions, globalSettingsActions]);

  return (
    <Router>
      <Navbar
        bg="dark"
        variant="dark"
        expand="lg"
        className="fixed-top header-gradient"
      >
        <Navbar.Brand href="/">
          <Logo
            className="d-inline"
            svgClassName="logo mr-3"
            letterColor="white"
            arrowColor="red"
            size="32px"
          />
          OmegaGraf
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <HeaderNav session={globalState} />
        </Navbar.Collapse>
      </Navbar>

      <AppliedRoutes />

      <Footer />
    </Router>
  );
}
