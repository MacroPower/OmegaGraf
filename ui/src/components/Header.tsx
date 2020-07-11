import React from 'react';
import Nav from 'react-bootstrap/Nav';
import { Session } from './Session';
import Routes, { RoutedLink, RouteIdentifiers } from './Routes';
import { Settings } from './settings/Settings';

function HeaderRoutedLinks(
  session: Session,
): {
  nav: JSX.Element[];
  login: JSX.Element[];
  logout: JSX.Element[];
} {
  const nav = Routes.filter(
    (route) => route.hidden === false && session && session.apiKey,
  ).map((route, i) => (
    <RoutedLink
      key={i}
      label={route.label}
      to={route.path}
      activeOnlyWhenExact={route.exact}
    />
  ));

  const login = Routes.filter(
    (route) => route.id === RouteIdentifiers.Login,
  ).map((route, i) => (
    <RoutedLink
      key={i}
      to={route.path}
      label={route.label}
      activeOnlyWhenExact={route.exact}
    />
  ));

  const logout = Routes.filter(
    (route) => route.id === RouteIdentifiers.Logout,
  ).map((route, i) => (
    <RoutedLink
      key={i}
      to={route.path}
      label={route.label}
      activeOnlyWhenExact={route.exact}
    />
  ));

  return { nav, login, logout };
}

export default function HeaderNav(props: {
  session: Session;
  settings: Settings;
}) {
  const { nav, login, logout } = HeaderRoutedLinks(props.session);

  return props.session.apiKey ? (
    <>
      <Nav className="mr-auto">{nav}</Nav>
      <Nav>
        <p className="p-2 m-0 text-light">
          Connected to: <b>{props.settings.Config.Hostname}</b>
        </p>
        {logout}
      </Nav>
    </>
  ) : (
    <>
      <Nav className="mr-auto">{login}</Nav>
    </>
  );
}
