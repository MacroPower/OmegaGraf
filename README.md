<p align="center">
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-1-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
  <img src="docs/branding/logo/dist/logo_name_g_1024.png" width="256px" height="256px">
</p>

[![Build Status](https://travis-ci.com/OmegaGraf/OmegaGraf.svg?branch=master)](https://travis-ci.com/OmegaGraf/OmegaGraf)
![GitHub tag (latest by date)](https://img.shields.io/github/v/tag/OmegaGraf/OmegaGraf)
![GitHub All Releases](https://img.shields.io/github/downloads/OmegaGraf/OmegaGraf/total)
![Libraries.io dependency status for GitHub repo](https://img.shields.io/librariesio/github/OmegaGraf/OmegaGraf)
![GitHub issues](https://img.shields.io/github/issues-raw/OmegaGraf/OmegaGraf)
![GitHub closed issues](https://img.shields.io/github/issues-closed-raw/OmegaGraf/OmegaGraf)
![GitHub](https://img.shields.io/github/license/OmegaGraf/OmegaGraf)

## Overview

OmegaGraf is a **vCenter monitoring deployment solution** that is incredibly light, lightning fast, and dead simple. Through a web interface, users may configure and deploy an entirely open-source and containerized software stack.

## Expo

<a href="https://cech.uc.edu/soitexpo.html"><img src="https://cech.uc.edu/soitexpo/expo-agenda/_jcr_content/main/textimage_273720174/image.img.png/1566481041698.png" width="256px"></a>

OmegaGraf will be showcased in UC's [IT Expo](https://cech.uc.edu/soitexpo.html), April 14, 2020.

|Current Progress||
|--|--|
|![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/OmegaGraf/OmegaGraf/1?color=green)|![GitHub milestone](https://img.shields.io/github/milestones/issues-open/OmegaGraf/OmegaGraf/1?label=Remaining&color=green)|
|![GitHub milestone](https://img.shields.io/github/milestones/progress-percent/OmegaGraf/OmegaGraf/2)|![GitHub milestone](https://img.shields.io/github/milestones/issues-open/OmegaGraf/OmegaGraf/2?label=Remaining)|

## Features

In minutes, OmegaGraf provides the following features and more:

<p align="left">
  <img src="docs/branding/graphs/dist/features_1440.png" width="600px">
</p>

## Requirements

For help installing requirements, you can use an [install script](https://github.com/OmegaGraf/OmegaGraf/tree/master/install).

Othwerwise, please install `docker-ce` and any prereqs.

OmegaGraf is supported on Windows and Linux. Our testing is done using Xenial, but other distributions should work just fine. We do not currently support MacOS, but if you're up for working with the C# Docker package on MacOS, feel free to give it a try!

## SSL

To run OmegaGraf with SSL, you will need to generate a certificate. To generate a self-signed cert, you can install `dotnet-sdk-3.1` and run `dotnet dev-certs https`.

## Using OmegaGraf

<p align="left">
  <img src="docs/branding/graphs/dist/flow_1440_run_detailed.png" width="500px">
</p>

1. Run OmegaGraf application
2. Visit printed URL and enter secure code
3. If you're just testing, check 'Use simulation'
4. Click 'Deploy'

After your deployment completes, you may start using the monitoring stack by visiting Grafana at `http://{hostname}:3000`, or whatever port you chose in the deployment options, and entering `admin`/`admin`. You will be prompted to set a new password on first login.

## Reconfigure & Redeploy

To make changes to any container created by OmegaGraf, you can run with the `--reset` switch. This will remove the containers, but will not delete any data (e.g. time series data, custom dashboards/datasources). You may then re-run the deployment process without issue. Note that currently, we cannot configure Grafana if you have changed the default password. A fix for this is coming soon.

## Comparison

OmegaGraf was inspired by several other products. We thought we could build on some of their accomplishments, any create something that was more dynamic, simpler to use, and easier to maintain.

|Feature|OmegaGraf|SexiGraf|
|-------|---------|--------|
|Containerized| :heavy_check_mark: | :heavy_multiplication_x: |
|Integration with existing products| :heavy_check_mark: | :heavy_multiplication_x: |
|Removable after deployment| :heavy_check_mark: | :heavy_multiplication_x: |
|Self-Monitoring| :heavy_check_mark: | :heavy_check_mark: |
|Collection| Telegraf | Perl |
|TSDB| Prometheus | Graphite |
|Display| Grafana | Modified Grafana |

## Philosophy

* OmegaGraf should only serve as a way to configure and deploy other products.
  * OmegaGraf should make no unsupported changes to any products in the stack.
  * OmegaGraf should support upgrading each individual container at-will, e.g. via Watchtower.
  * While other solutions fall behind on patches, OmegaGraf's deployments should always be up-to-date.
* OmegaGraf should run only during the deployment phase, and should take no actions in any other circumstance.
  * Users should feel free to `rm OmegaGraf.sh` after deploying.
* While the UI is a core tenant, users should always have the option to deploy with OmegaGraf using code.

## Credits

OmegaGraf uses the following technologies:

<p align="left">
  <img src="docs/branding/graphs/dist/tech_1440.png" width="400px">
</p>

With assets from [Font Awesome](https://fontawesome.com/), [Proxima Nova](https://www.marksimonson.com/fonts/view/proxima-nova), and [Bootstrap](https://getbootstrap.com/) via [React Bootstrap](https://react-bootstrap.github.io/).

View all our dependencies on [libraries.io](https://libraries.io/github/OmegaGraf/OmegaGraf).

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://stackoverflow.com/users/4868262/jacob-colvin"><img src="https://avatars1.githubusercontent.com/u/5648814?v=4" width="100px;" alt=""/><br /><sub><b>Jacob Colvin</b></sub></a><br /><a href="https://github.com/OmegaGraf/OmegaGraf/commits?author=MacroPower" title="Code">💻</a> <a href="#infra-MacroPower" title="Infrastructure (Hosting, Build-Tools, etc)">🚇</a> <a href="#maintenance-MacroPower" title="Maintenance">🚧</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!