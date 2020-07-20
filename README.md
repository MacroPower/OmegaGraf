<!-- markdownlint-disable -->
<p align="center">
  <a href="#"><img src="docs/branding/logo/dist/logo_name_g_1024.png" width="256px" height="256px" /></a>
</p>

<h2 align="center">Fast & Easy vCenter Monitoring Deployment</h2>

<p align="center">
  <a href="https://github.com/OmegaGraf/OmegaGraf/actions"><img alt="Build Status" src="https://github.com/OmegaGraf/OmegaGraf/workflows/Build/badge.svg"></a>
  <a href="https://github.com/OmegaGraf/OmegaGraf/releases"><img alt="GitHub tag (latest by date)" src="https://img.shields.io/github/v/tag/OmegaGraf/OmegaGraf"></a>
  <a href="https://github.com/OmegaGraf/OmegaGraf/releases"><img alt="GitHub All Releases" src="https://img.shields.io/github/downloads/OmegaGraf/OmegaGraf/total"></a>
  <a href="https://bestpractices.coreinfrastructure.org/projects/3874"><img src="https://bestpractices.coreinfrastructure.org/projects/3874/badge"></a>
  <a href="#contributors-"><img alt="All Contributors" src="https://img.shields.io/badge/all_contributors-3-blue.svg"></a>
  <a href="#"><img alt="License" src="https://img.shields.io/github/license/OmegaGraf/OmegaGraf"></a>
</p>

_OmegaGraf_ is a fast and simple **vCenter monitoring deployment solution**.
It automatically composes Telegraf, Prometheus, and Grafana containers
to your specifications. Once you're done, you can delete _OmegaGraf_ and keep
your vCenter monitoring running via your management tooling of choice.

When started, _OmegaGraf_ runs a **simple web-interface** that can be used to
orchistrate deployments. Just run the binary, visit the _OmegaGraf_ homepage,
and start a deployment. Monitoring can be up and running in under 5 minutes.
The amount of configuration required on your part can be as little as providing
a readonly account!

Alternatively, _OmegaGraf_ can be controlled via its API, so more technical
users can **store and version configuration**. Simply re-run _OmegaGraf_ and
supply your old or updated JSON data.

## Getting Started

<!-- markdownlint-enable -->

- **[Getting Started](docs/getting-started.md)**
- [Walkthrough (with Screenshots)](docs/walkthrough.md)
- [API](docs/api.md)
- [Architecture](docs/architecture.md)

## Features

- Deploys a Telegraf, Prometheus and Grafana stack in just minutes.
- Simple [React Web UI](ui) to manage configuration and deployment.
- Suite of arguments and options to customize behavior.
- Deserializes and actions on configuration as JSON data.
- API documented via the OpenAPI Specification.
- Fully testable through the [vCenter Simulator](https://github.com/OmegaGraf/docker-vcsim) container.
- Automatically configures Grafana and adds a set of [curated dashboards](grafana).
- Can be set up with just one command using our [install script](install).

## Philosophy

- _OmegaGraf_ should only serve as a way to configure and deploy other products.
  - _OmegaGraf_ should make no unsupported changes to any products in the stack.
  - _OmegaGraf_ should support upgrading each individual container at-will, e.g. via Watchtower.
  - While other solutions fall behind on patches, _OmegaGraf_'s deployments should always be up-to-date.
- _OmegaGraf_ should run only during the deployment phase, and should take no actions in any other circumstance.
  - Users should feel free to remove all _OmegaGraf_ binaries after deploying.
- While the UI is a core tenant, _OmegaGraf_ should always have a feature-complete and documented API.

## Credits

_OmegaGraf_ uses the following technologies:

<p align="left">
  <img src="docs/branding/graphs/dist/tech_1440.png" width="400px">
</p>

With assets from [Font Awesome](https://fontawesome.com/),
[Proxima Nova](https://www.marksimonson.com/fonts/view/proxima-nova), and
[Bootstrap](https://getbootstrap.com/) via [React Bootstrap](https://react-bootstrap.github.io/).

View all our dependencies on [libraries.io](https://libraries.io/github/OmegaGraf/OmegaGraf).

## Contributing

Check out the [Contributing Guide](CONTRIBUTING.md).

## Contributors

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/MacroPower"><img src="https://avatars1.githubusercontent.com/u/5648814?v=4" width="100px;" alt=""/><br /><sub><b>Jacob Colvin</b></sub></a><br /><a href="https://github.com/OmegaGraf/OmegaGraf/commits?author=MacroPower" title="Code">💻</a> <a href="#infra-MacroPower" title="Infrastructure (Hosting, Build-Tools, etc)">🚇</a> <a href="#maintenance-MacroPower" title="Maintenance">🚧</a></td>
    <td align="center"><a href="https://github.com/curriemw"><img src="https://avatars2.githubusercontent.com/u/2603635?v=4" width="100px;" alt=""/><br /><sub><b>Matt Currie</b></sub></a><br /><a href="#projectManagement-curriemw" title="Project Management">📆</a> <a href="#design-curriemw" title="Design">🎨</a></td>
    <td align="center"><a href="https://github.com/xvDylan"><img src="https://avatars0.githubusercontent.com/u/55466545?v=4" width="100px;" alt=""/><br /><sub><b>xvDylan</b></sub></a><br /><a href="#security-xvDylan" title="Security">🛡️</a> <a href="https://github.com/OmegaGraf/OmegaGraf/commits?author=xvDylan" title="Tests">⚠️</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors)
specification. Contributions of any kind are welcome!
