import '../styles/ghost.scss';
import React from 'react';

type polyPoint = number[][];

function Polygon(props: { points: polyPoint; scale?: number }) {
  const scale = props.scale;

  const s = props.points.map((point: number[]) => {
    return point
      .map(x => {
        if (scale) {
          return x / scale;
        }
        return x;
      })
      .join(' ');
  });

  return <polygon points={s.join(', ')} />;
}

export default function PacmanGhost() {
  return (
    <div className="ghost inky mt-4">
      <div className="ghost__body">
        <svg>
          <Polygon
            scale={2}
            points={[
              [0, 24],
              [4, 24],
              [4, 12],
              [8, 12],
              [8, 8],
              [12, 8],
              [12, 4],
              [20, 4],
              [20, 0],
              [36, 0],
              [36, 4],
              [44, 4],
              [44, 8],
              [48, 8],
              [48, 12],
              [52, 12],
              [52, 24],
              [56, 24],
              [56, 48],
              [0, 48]
            ]}
          />
        </svg>
      </div>
      <div className="ghost__eye--left">
        <svg>
          <Polygon
            scale={2}
            points={[
              [4, 0],
              [12, 0],
              [12, 4],
              [16, 4],
              [16, 16],
              [12, 16],
              [12, 20],
              [4, 20],
              [4, 16],
              [0, 16],
              [0, 4],
              [4, 4]
            ]}
          />
        </svg>
      </div>
      <div className="ghost__eye--right">
        <svg>
          <Polygon
            scale={2}
            points={[
              [4, 0],
              [12, 0],
              [12, 4],
              [16, 4],
              [16, 16],
              [12, 16],
              [12, 20],
              [4, 20],
              [4, 16],
              [0, 16],
              [0, 4],
              [4, 4]
            ]}
          />
        </svg>
      </div>
      <div className="ghost__feet" />
    </div>
  );
}
