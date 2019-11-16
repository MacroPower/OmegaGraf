import React from "react";
import { UseGlobalSession, UseGlobalSettings } from "../components/Global";

export default function About() {
  const [globalSession, globalSessionActions] = UseGlobalSession();
  const [globalSettings, globalSettingsActions] = UseGlobalSettings();

    return (
      <main role="main" className="container">
        <h2>About</h2>
        <hr />
        Your session data:
        <pre>{JSON.stringify(globalSession, null, 1)}</pre>
        <hr />
        Your global data:
        <pre>{JSON.stringify(globalSettings, null, 1)}</pre>
      </main>
    );
}
