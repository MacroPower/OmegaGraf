<p align="center">
  <img src="docs/branding/logo_128.png">
</p>
<h2 align="center">OmegaGraf</h2>

OmegaGraf is an entirely open-source and containerized solution, configured and deployed through a simple web interface. In minutes, OmegaGraf provides the following features and more:

<p align="center">
  <img src="docs/branding/flow_1440_features.png" width="500px">
</p>

## Requirements

[Click Here](https://github.com/OmegaGraf/install) for automated installers.

- Linux (prod/dev) or Windows OS (dev)
- Docker (prod/dev) or Docker Desktop (dev)
- OpenSSL (prod)

## Using OmegaGraf

<p align="left">
  <img src="docs/branding/flow_1440_run_detailed.png" width="500px">
</p>

1. Run OmegaGraf application
2. Visit printed URL and enter secure code
3. If you're just testing, check 'Use simulation'
4. Click 'Deploy'

After your deployment completes, you may start using the monitoring stack by visiting Grafana at `http://{hostname}:3000` and entering `admin`/`admin`. You will be prompted to set a new password on first login.

## Create a release

```
run ./build.ps1
--> build/{env}/
```

## Credits

OmegaGraf uses the following technologies:

<p align="left">
  <img src="docs/branding/tech_1440.png" width="400px">
</p>

With assets from [Font Awesome](https://fontawesome.com/), [Proxima Nova](https://www.marksimonson.com/fonts/view/proxima-nova), and [Bootstrap](https://getbootstrap.com/) via [React Bootstrap](https://react-bootstrap.github.io/).
