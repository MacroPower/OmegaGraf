import Cookies from "js-cookie";

export type Session = {
  endpoint: string | undefined;
  apiKey: string | undefined;
};

function IsSession(arg: any): arg is Session {
  return (
    arg.endpoint &&
    arg.apiKey
  );
}

export const defaultSession: Session = {
  endpoint: undefined,
  apiKey: undefined
};

//

export const removeSessionCookie = (): void => {
  Cookies.remove("session");
};

export const setSessionCookie = (session: Session): void => {
  removeSessionCookie();
  Cookies.set("session", session, { expires: 1 });
};

export const getSessionCookie = (): Session | null => {
  const sessionCookie = Cookies.get("session");

  if (sessionCookie !== undefined) {
    const session = JSON.parse(sessionCookie);
    if (IsSession(session)) {
      return session;
    }
  }

  return null;
};
