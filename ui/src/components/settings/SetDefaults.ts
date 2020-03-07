import { setSessionCookie, defaultSession, Session } from '../Session';
import {
  GlobalSettingsActions,
  GlobalGrafanaActions,
  GlobalSimActions
} from '../Global';

export default function setDefaults(
    globalState: Session,
    globalSettingsActions: GlobalSettingsActions,
    globalGrafanaActions: GlobalGrafanaActions,
    globalSimActions: GlobalSimActions
) {
  const url = globalState.endpoint + '/example';
  console.log('Get settings from ' + url);

  fetch(url, {
    method: 'GET',
    headers: {
      Accept: 'application/json'
    }
  })
    .then(response => response.json())
    .then(data => globalSettingsActions.setSettings(data))
    .then(x => globalGrafanaActions.setGrafana({ Active: true }))
    .then(x => globalSimActions.setSim({ Active: false, Quantity: 0 }))
    .catch((e: Error) => {
      console.log(e.message);
      setSessionCookie(defaultSession);
    });
}
