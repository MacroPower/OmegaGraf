import React from "react";
import { Session } from "./Session";

type ContextProps = { 
    session: Session,
    setKey: any
  };

const AuthContext = 
  React.createContext<Partial<ContextProps>>({});

export default AuthContext;