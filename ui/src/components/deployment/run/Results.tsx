import React from 'react';

export default function Results(props: {
  url: string;
  port: string;
  app: string;
  message?: string;
  path?: string;
}) {
  const { app, url, port, message, path } = props;
  const location = `${url}:${port}`;
  return (
    <div className="mb-3 p-3 deploy-results">
      <h5>{`Your new ${app} instance:`}</h5>
      <hr />
      <a href={location + (path || '')}>{location}</a>
      {message && <hr />} {message}
    </div>
  );
}
