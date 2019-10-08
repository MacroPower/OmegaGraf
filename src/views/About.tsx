import React, { Component } from "react";
import AuthContext from "../components/Context";

export default class About extends Component {
  static contextType = AuthContext;

  render() {
    return (
      <main role="main" className="container">
        <h2>About</h2>
        <hr />
        Your session data:
        <pre>{JSON.stringify(this.context, null, 1)}</pre>
      </main>
    );
  }
}
