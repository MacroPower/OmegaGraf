import Cookies from "js-cookie";

export type Session = {
  endpoint: string;
  apiKey: string;
  username: string;
};

function IsSession(arg: any): arg is Session {
  return (
    arg.endpoint &&
    arg.apiKey &&
    arg.username
  );
}

export const setSessionCookie = (session: Session): void => {
  removeSessionCookie();
  Cookies.set("session", session, { expires: 1 });
};

export const removeSessionCookie = (): void => {
  Cookies.remove("session");
};

export const getSessionCookie: any = () => {
  const sessionCookie = Cookies.get("session");

  if (sessionCookie !== undefined) {
    const session = JSON.parse(sessionCookie);
    if (IsSession(session)) {
      return session;
    }
  }

  return null;
};
