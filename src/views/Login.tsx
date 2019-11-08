import React, { useState } from "react";
import { Button, Form } from "react-bootstrap";
import { useGlobal, setSessionCookie } from "../components/Session";
import { Redirect } from "react-router";

export default function Login() {
  const [globalState, globalActions] = useGlobal();
  const [key, setKey] = useState("");
  const [toHome, redirect] = useState(false);

  const submit = (e: any) => {
    e.preventDefault();

    const session = {
      endpoint: "",
      apiKey: key
    };

    // TODO: VALIDATE API KEY HERE

    setSessionCookie(session);
    globalActions.setValue(session);
    redirect(true);
  };

  return (
    <main role="main" className="container">
      {toHome && <Redirect to="/" />}
      <h2>Login</h2>
      <Form onSubmit={submit}>
        <Form.Group controlId="formKey">
          <Form.Label>OmegaKey</Form.Label>
          <Form.Control
            value={key}
            onChange={(e: any) => setKey(e.target.value)}
            type="text"
            placeholder="Unique Key"
          />
        </Form.Group>
        <Button variant="primary" type="submit">
          Submit
        </Button>
      </Form>
    </main>
  );
}
