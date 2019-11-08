import React from "react";
import { useGlobal } from "../components/Session";

export default function About() {
  const [globalState, globalActions] = useGlobal();
    return (
      <main role="main" className="container">
        <h2>About</h2>
        <hr />
        Your session data:
        <pre>{JSON.stringify(globalState, null, 1)}</pre>
      </main>
    );
}
