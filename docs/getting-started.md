# Getting Started

## Auto Install

If you're using Debian, you can use an [install script](https://github.com/OmegaGraf/OmegaGraf/tree/master/install).

## Requirements

Othwerwise, please install `docker-ce` and any prereqs.

OmegaGraf is supported on Windows and Linux. Our testing is done using Xenial, but other distributions should work just fine. We do not currently support MacOS, but if you're up for working with the C# Docker package on MacOS, feel free to give it a try!

### SSL

To run OmegaGraf with SSL, you will need to generate a certificate. To generate a self-signed cert, you can install `dotnet-sdk-3.1` and run `dotnet dev-certs https`. This process will eventually be improved.

## Using OmegaGraf

1. Run the OmegaGraf application.
2. Visit printed URL and enter your secure code.
3. Enter your vCenter details. If you're just testing, check 'Use simulation'.
4. Click 'Deploy'.

After your deployment completes, you may start using the monitoring stack by visiting Grafana at `http://{hostname}:3000`, or whatever port you chose in the deployment options, and entering `admin`/`admin`. You will be prompted to set a new password on first login.

For a more detailed walkthrough with screenshots, take a look at [our walkthrough](walkthrough.md).

## Reconfigure & Redeploy

To make changes to any container created by OmegaGraf, you can run with the `--reset` switch. This will remove the containers, but will not delete any data (e.g. time series data, custom dashboards/datasources). You may then re-run the deployment process without issue. Note that currently, we cannot configure Grafana if you have changed the default password. A fix for this is coming soon.
