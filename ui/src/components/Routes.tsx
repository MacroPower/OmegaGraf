import React, { Component, useContext } from "react";

// eslint-disable-next-line
import {
  BrowserRouter as Router,
  Route,
  Link,
  Switch,
  Redirect,
  match
} from "react-router-dom";
import AuthContext from "./Context";

import Home from "../views/Home";
import Four04 from "../views/404";
import About from "../views/About";
import Login from "../views/Login";
import { UseGlobalSession } from "./Global";
import Deploy from "../views/Deploy";

export enum RouteIdentifiers {
  Login,
  Logout
}

type Routed = {
  id?: RouteIdentifiers;
  path: string;
  label: string;
  exact: boolean;
  hidden: boolean;
  requiresAuth: boolean;
  component: any;
};

const Routes: Routed[] = [
  {
    path: "/",
    label: "Home",
    exact: true,
    hidden: false,
    requiresAuth: true,
    component: Home
  },
  {
    path: "/about",
    label: "About",
    exact: false,
    hidden: false,
    requiresAuth: true,
    component: About
  },
  {
    path: "/deploy",
    label: "Deploy",
    exact: false,
    hidden: true,
    requiresAuth: true,
    component: Deploy
  },
  {
    id: RouteIdentifiers.Login,
    path: "/login",
    label: "Login",
    exact: false,
    hidden: true,
    requiresAuth: false,
    component: Login
  }
];

export function AppliedRoutes() {
  const [globalState, globalActions] = UseGlobalSession();

  // Filter if requiresAuth is true and session is invalid
  const routes = Routes.map(route => {
    if(globalState.apiKey || !route.requiresAuth) {
      return <Route exact={route.exact} path={route.path} component={route.component} />
    }
  });

  return (
    <Switch>
      {routes}
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
          className={"nav-link " + (match ? "active" : "")}
          to={this.props.to}
          aria-selected={match ? "true" : "false"}
        >
          {this.props.label}
        </Link>
      )
    };
    return <Route {...data} />;
  }
}

export default Routes;
