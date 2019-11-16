import React from "react";
import { UseGlobalSession } from "../components/Global";

export default function About() {
  const [globalState, globalActions] = UseGlobalSession();
    return (
      <main role="main" className="container">
        <h2>About</h2>
        <hr />
        Your session data:
        <pre>{JSON.stringify(globalState, null, 1)}</pre>
      </main>
    );
}
