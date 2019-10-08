import React, { Fragment, Component } from "react";
import Nav from "react-bootstrap/Nav";
import { Session } from "./Session";
import Routes, { RoutedLink, RouteIdentifiers } from "./Routes";

function HeaderRoutedLinks(
  session: Session
): {
  nav: JSX.Element[];
  login: JSX.Element[];
  logout: JSX.Element[];
} {
  const nav = Routes.filter(
    route => route.hidden === false && session && session.apiKey
  ).map(route => (
    <RoutedLink
      label={route.label}
      to={route.path}
      activeOnlyWhenExact={route.exact}
    />
  ));

  const login = Routes.filter(route => route.id === RouteIdentifiers.Login).map(
    route => (
      <RoutedLink
        to={route.path}
        label={route.label}
        activeOnlyWhenExact={route.exact}
      />
    )
  );

  const logout = Routes.filter(
    route => route.id === RouteIdentifiers.Logout
  ).map(route => (
    <RoutedLink
      to={route.path}
      label={route.label}
      activeOnlyWhenExact={route.exact}
    />
  ));

  return { nav, login, logout };
}

export default class HeaderNav extends Component<{
  session: Session;
}> {
  render() {
    const { nav, login, logout } = HeaderRoutedLinks(this.props.session);

    return this.props.session.apiKey ? (
      <Fragment>
        <Nav className="mr-auto">{nav}</Nav>
        <Nav>
          <p className="p-2 m-0 text-light">
            Logged in as: <b>{this.props.session.username}</b>
          </p>
          {logout}
        </Nav>
      </Fragment>
    ) : (
      <Fragment>
        <Nav className="mr-auto">{login}</Nav>
      </Fragment>
    );
  }
}
