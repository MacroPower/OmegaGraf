import { UseGlobalSession } from '../../Global';

export default function DeployRequest(endpoint: string, location: string, body: string) {
  const url = endpoint + '/' + location;

  return fetch(url, {
    method: 'POST',
    body: body
  })
    .then(x => x.json())
    .then(x => console.log(x))
    .catch(x => {
      console.error(x);
      throw x;
    });
}
