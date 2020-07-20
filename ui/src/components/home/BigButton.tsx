import React from 'react';

interface State {
  disabled?: boolean;
  onClick?: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
}

export default function BigButton(props: React.PropsWithChildren<State>) {
  const { disabled, children, onClick } = props;
  return (
    <button className="big-btn" disabled={disabled} onClick={onClick}>
      {children}
      <div
        className={disabled ? 'big-btn-s big-btn-s-disabled' : 'big-btn-s'}
      />
    </button>
  );
}
