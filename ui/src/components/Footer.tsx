import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFileCode } from '@fortawesome/free-solid-svg-icons';
import { faGithub } from '@fortawesome/free-brands-svg-icons';
import { Container, Row, Col } from 'react-bootstrap';
import Logo from '../data/Logo';

export default function Footer() {
  const git = 'https://github.com/';
  const repo = 'OmegaGraf/OmegaGraf';
  const dev = 'MacroPower';
  const style = 'social';
  const shields = 'https://img.shields.io/github';
  const web = 'https://OmegaGraf.github.io';

  return (
    <div className="footer d-flex flex-column lh-1">
      <Container>
        <Row className="justify-content-md-center">
          <Col md={4} sm={4} xs={5} className="footer-logo">
            <a href={web} target="_BLANK">
              <Logo
                className="d-inline"
                svgClassName="logo mr-3"
                letterColor="var(--black)"
                arrowColor="var(--black)"
                size="1.5rem"
              />
              OmegaGraf
            </a>
          </Col>
          <Col md={4} sm={2} xs={1}>
            <a
              href={git + repo}
              target="_BLANK"
              className="text-decoration-none"
            >
              <FontAwesomeIcon icon={faGithub} className="footer-icon" />
            </a>
          </Col>
          <Col md={4} sm={6} xs={6}>
            <a href={git + repo} target="_BLANK">
              <img
                className="footer-shield mr-1"
                alt="GitHub stars"
                src={shields + '/stars/' + repo + '?style=' + style}
              />
            </a>
            <a href={git + dev} target="_BLANK">
              <img
                className="footer-shield ml-1"
                alt="GitHub followers"
                src={shields + '/followers/' + dev + '?style=' + style}
              />
            </a>
          </Col>
        </Row>
        <hr />
        <Row className="justify-content-md-left mt-4">
          <Col md={2} className="footer-swagger">
            <a
              href="/swagger-ui"
              target="_BLANK"
              className="text-decoration-none"
            >
              <FontAwesomeIcon icon={faFileCode} /> API
            </a>
          </Col>
          <Col md={8}>
            <i>OmegaGraf is free and open source software</i>
            <a href={git + repo + '/blob/master/LICENSE'} target="_BLANK">
              <img
                className="ml-2"
                alt="GitHub"
                src={shields + '/license/' + repo + '?style=flat-square'}
              />
            </a>
          </Col>
        </Row>
      </Container>
    </div>
  );
}
