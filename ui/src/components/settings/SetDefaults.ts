import { setSessionCookie, defaultSession, Session } from '../Session';
import {
  GlobalSettingsActions,
  GlobalGrafanaActions,
  GlobalSimActions,
} from '../Global';

export default function setDefaults(
  globalState: Session,
  globalSettingsActions: GlobalSettingsActions,
  globalGrafanaActions: GlobalGrafanaActions,
  globalSimActions: GlobalSimActions,
) {
  const url = globalState.endpoint + '/default';
  console.log('Get settings from ' + url);

  fetch(url, {
    method: 'GET',
    headers: {
      Authorization: '' + globalState.apiKey,
      Accept: 'application/json',
    },
  })
    .then((response) => response.json())
    .then((data) => globalSettingsActions.setSettings(data))
    .then(() => globalGrafanaActions.setGrafana({ Active: true }))
    .then(() => globalSimActions.setSim({ Active: false, Quantity: 0 }))
    .catch((e: Error) => {
      console.log(e.message);
      setSessionCookie(defaultSession);
    });
}
