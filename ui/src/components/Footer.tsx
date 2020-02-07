import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleDoubleRight } from "@fortawesome/free-solid-svg-icons";

export default function Footer() {
  return (
    <div className="footer d-flex flex-column">
      <div className="container">
        <div className="row justify-content-md-center">
            <i>OmegaGraf is completely free and open source.</i>
            <FontAwesomeIcon icon={faAngleDoubleRight} className="ml-1" />
            <i>MIT 2.0</i>
        </div>
      </div>
    </div>
  );
}