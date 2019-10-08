import React, { Component } from "react";
import "./App.css";
import { BrowserRouter as Router } from "react-router-dom";
import { faReact } from "@fortawesome/free-brands-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Session } from "./components/Session";
import { AppliedRoutes } from "./components/Routes";
import AuthContext from "./components/Context";
import Footer from "./components/Footer";
import HeaderNav from "./components/Header";
import Navbar from "react-bootstrap/Navbar";

export default class App extends Component<{}, { session: Session }> {
  state = {
    session: {
      endpoint: "https://localhost:5001",
      apiKey: "string",
      username: "user"
    }
  };

  render() {
    return (
      <Router>
        <Navbar
          bg="dark"
          variant="dark"
          expand="lg"
          className="fixed-top header-gradient"
        >
          <Navbar.Brand href="/">
            <FontAwesomeIcon icon={faReact} className="mr-1" />
            OmegaGraf
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <HeaderNav session={this.state.session} />
          </Navbar.Collapse>
        </Navbar>

        <AuthContext.Provider value={this.state}>
          <AppliedRoutes />
        </AuthContext.Provider>

        <Footer />
      </Router>
    );
  }
}
