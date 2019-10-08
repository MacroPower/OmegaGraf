import React from "react";
import AuthContext from "../components/Context";

export default class Home extends React.Component {
  static contextType = AuthContext;

  render() {
    return (
      <main role="main" className="container">
        <h2>Home</h2>
        Hello World!
      </main>
    );
  }
}
