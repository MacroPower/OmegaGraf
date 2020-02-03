import React, { useState } from "react";
import { Card, ProgressBar, Button } from "react-bootstrap";

function DeployVCSim() {
    const [progress, setProgress] = useState(0);

    return (
      <Card style={{ width: "18rem" }}>
        <Card.Body>
          <Card.Title>vCenter Simulator</Card.Title>
          <Card.Subtitle className="mb-2 text-muted">
            Simulates the vCenter API
          </Card.Subtitle>
          <Card.Text>
          <Button variant="primary" onClick={() => setProgress(progress + 1)}>Deploy</Button>
            <ProgressBar variant="info" now={progress} />
          </Card.Text>
        </Card.Body>
      </Card>
    );
}

export default DeployVCSim;