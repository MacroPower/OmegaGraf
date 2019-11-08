import React, { Component } from "react";
import { Button } from "react-bootstrap";
import DeployVCSim from "../components/deployment/VCSim";

export default function Home() {
    return (
      <main role="main" className="container">
        <h2>Home</h2>
        <br />
        <Button variant="primary">
          Deploy
        </Button>
        {DeployVCSim()}
      </main>
    );
}
