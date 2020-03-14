import React, { Component, useEffect, useState } from 'react';

import {
  Route,
  Link,
  Switch,
  Redirect,
  // eslint-disable-next-line
  match
} from 'react-router-dom';

import { UseGlobalSession } from './Global';

import Home from '../views/Home';
import Four04 from '../views/404';
import About from '../views/About';
import Login from '../views/Login';
import Deploy from '../views/Deploy';
import SimpleForm from './deployment/forms/SimpleForm';
import NormalForm from './deployment/forms/NormalForm';
import AdvancedForm from './deployment/forms/AdvancedForm';

export enum RouteIdentifiers {
  Login,
  Logout
}

export type Routed = {
  id?: RouteIdentifiers;
  path: string;
  label: string;
  exact: boolean;
  hidden: boolean;
  requiresAuth: boolean;
  component: any;
};

const optionalAuth = true;

const Routes: Routed[] = [
  {
    path: '/',
    label: 'Home',
    exact: true,
    hidden: false,
    requiresAuth: optionalAuth,
    component: Home
  },
  {
    path: '/about',
    label: 'About',
    exact: false,
    hidden: false,
    requiresAuth: optionalAuth,
    component: About
  },
  {
    path: '/deploy',
    label: 'Deploy',
    exact: false,
    hidden: true,
    requiresAuth: optionalAuth,
    component: Deploy
  },
  {
    id: RouteIdentifiers.Login,
    path: '/login',
    label: 'Login',
    exact: false,
    hidden: true,
    requiresAuth: false,
    component: Login
  },
  {
    path: '/form/simple',
    label: 'Simple',
    exact: false,
    hidden: true,
    requiresAuth: optionalAuth,
    component: SimpleForm
  },
  {
    path: '/form/normal',
    label: 'Normal',
    exact: false,
    hidden: true,
    requiresAuth: optionalAuth,
    component: NormalForm
  },
  {
    path: '/form/advanced',
    label: 'Advanced',
    exact: false,
    hidden: true,
    requiresAuth: optionalAuth,
    component: AdvancedForm
  }
];

export function AppliedRoutes() {
  const [globalState] = UseGlobalSession();

  const [routes, setRoutes] = useState<Routed[]>(
    Routes.filter(route => globalState.apiKey || !route.requiresAuth)
  );

  useEffect(() => {
    setRoutes(
      Routes.filter(route => globalState.apiKey || !route.requiresAuth)
    );
  }, [globalState]);

  return (
    <Switch>
      {routes.map((route, i) => (
        <Route
          key={'AppliedRoute' + i}
          exact={route.exact}
          path={route.path}
          component={route.component}
        />
      ))}
      {!globalState.apiKey && <Redirect to="/login" />}
      <Route component={Four04} />
    </Switch>
  );
}

export class RoutedLink extends Component<{
  label: string;
  to: string;
  activeOnlyWhenExact: boolean;
}> {
  render() {
    const data = {
      label: this.props.label,
      path: this.props.to,
      exact: this.props.activeOnlyWhenExact,
      children: ({ match }: { match: match }) => (
        <Link
          className={'nav-link ' + (match ? 'active' : '')}
          to={this.props.to}
          aria-selected={match ? 'true' : 'false'}
        >
          {this.props.label}
        </Link>
      )
    };
    return <Route {...data} />;
  }
}

export default Routes;
