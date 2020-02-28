import React, { useState, Component } from 'react';
import { Button, Container, Row, Col } from 'react-bootstrap';
import { Route, Link, match } from 'react-router-dom';
import OptionCard from '../OptionCard';
import BigButton from '../BigButton';

class RoutedLink extends Component<{
  to: string;
  disabled: boolean;
}> {
  render() {
    const data = {
      label: '',
      path: this.props.to,
      exact: false,
      children: ({ match }: { match: match }) => (
        <Link to={this.props.to} className="text-decoration-none">
          <BigButton disabled={this.props.disabled}>Get Started</BigButton>
        </Link>
      )
    };
    return <Route {...data} />;
  }
}

export default function DeployForm() {
  const [direct, setDirect] = useState('');

  return (
    <Container>
      <Row className="mb-4 justify-content-md-center">
        <Col md={4}>
          <a onClick={() => setDirect('simple')}>
            <OptionCard clicked={direct === 'simple'} phase="1" />
          </a>
        </Col>
        <Col md={4}>
          <a onClick={() => setDirect('normal')}>
            <OptionCard clicked={direct === 'normal'} phase="2" />
          </a>
        </Col>
        <Col md={4}>
          <a onClick={() => setDirect('advanced')}>
            <OptionCard clicked={direct === 'advanced'} phase="3" />
          </a>
        </Col>
      </Row>
      <br />
      <Row className="mt-4 justify-content-md-center">
        <RoutedLink to={'/form/' + direct} disabled={direct === ''} />
      </Row>
    </Container>
  );
}
