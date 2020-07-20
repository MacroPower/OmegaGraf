import React from 'react';
import DeployForm from '../components/home/Form';

export default function Home() {
  return (
    <main role="main" className="container">
      <br />
      <h1 className="home-title text-center">Welcome to OmegaGraf</h1>
      <h4 className="home-subtitle text-center">Let&apos;s get started.</h4>
      <br />
      <hr />
      <br />
      <DeployForm />
    </main>
  );
}
