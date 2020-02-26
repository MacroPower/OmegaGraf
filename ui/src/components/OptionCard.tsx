import React from 'react';
import { Card } from 'react-bootstrap';
import SettingsIcon from '../data/SettingsIcon';

const phases: {
  [key: string]: {
    icon: 'toolbox' | 'cog' | 'tools';
    color: string;
    title: string;
    description: string;
  };
} = {
  '1': {
    icon: 'cog',
    color: 'green',
    title: 'Default',
    description: 'Use the default configuration.'
  },
  '2': {
    icon: 'toolbox',
    color: 'orange',
    title: 'Minor Adjustments',
    description: 'Make common changes.'
  },
  '3': {
    icon: 'tools',
    color: 'red',
    title: 'Advanced Settings',
    description: 'Make more intricate changes.'
  }
};

export default function OptionCard(props: {
  clicked: boolean;
  phase: '1' | '2' | '3';
}) {
  const clicked = props.clicked;

  const { icon, color, title, description } = phases[props.phase];

  return (
    <>
      <Card
        className={
          clicked
            ? 'text-center home-card card-clicked'
            : 'home-card text-center'
        }
      >
        <Card.Body className="pb-0">
          <SettingsIcon
            icon={icon}
            className="mt-1 mb-1"
            svgClassName=""
            color={color}
            size="128px"
          />
        </Card.Body>
        <Card.Body>
          <Card.Title>{title}</Card.Title>
          <Card.Text>{description}</Card.Text>
        </Card.Body>
      </Card>
    </>
  );
}
