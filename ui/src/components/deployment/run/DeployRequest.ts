export default async function DeployRequest(
  endpoint: string,
  apiKey: string,
  location: string,
  body: any
) {
  const url = endpoint + '/' + location;

  try {
    const x = await fetch(url, {
      method: 'POST',
      body: JSON.stringify(body),
      headers: {
        Authorization: '' + apiKey,
        'Content-Type': 'application/json'
      }
    }).then(r => {
        if (r.ok) {
            return r.json()
        }

        throw new Error('API returned not OK')
    });
    return console.log(x);
  } catch (e) {
    console.error(e);
    throw e;
  }
}
