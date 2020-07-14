# Getting Started

## Installation

OmegaGraf is a portable binary. However, your system will need to have a few packages installed before you can successfully deploy.

OmegaGraf is supported on Windows and Linux. Our testing is done using Xenial, but other distributions should work just fine. We do not currently support MacOS, but if you're up for working with the C# Docker package on MacOS, feel free to give it a try!

**OmegaGraf currently uses the current _working directory_ to store container data.** Please keep this in mind. If you want to change this behavior, you can supply the `--path` argument along with an absolute or relative path to store data.

### Automatic

If you're using Debian, you can use an [install script](https://github.com/OmegaGraf/OmegaGraf/tree/master/install).

This script will install all dependencies, setup docker, setup SSL (optional), and it will download and run the latest OmegaGraf release.

### Manual

On Linux, please install `docker-ce` and any prerequisites. On Windows, you can use Docker Desktop.

To run OmegaGraf with SSL on Windows, you can simply specify the https protocol by using the `--host` parameter. On Linux, you will need to generate a certificate. To generate a self-signed cert, you can install `dotnet-sdk-3.1` and run `dotnet dev-certs https`. This process will eventually be improved.

## Using OmegaGraf

1. Run the OmegaGraf application.
2. Visit printed URL and enter your secure code.
3. Enter your vCenter details. If you're just testing, check 'Use simulation'.
4. Click 'Deploy'.

After your deployment completes, you may start using the monitoring stack by visiting Grafana at `http://{hostname}:3000`, or whatever port you chose in the deployment options, and entering `admin`/`admin`. You will be prompted to set a new password on first login.

For a more detailed walkthrough with screenshots, take a look at [our walkthrough](walkthrough.md).

## Advanced Usage

OmegaGraf has several parameters designed for more advanced users.

### Reconfigure & Redeploy

To make changes to any container created by OmegaGraf, you can run with the `--reset` or `--overwrite` switch. `--reset` will remove all OmegaGraf containers at start, `--overwrite` will remove containers as you redeploy them. Neither switch will delete any data (e.g. time series data, custom dashboards/datasources). If you set either one of these switches you should be able to re-run the deployment process without issue. Note that currently, we cannot always reconfigure Grafana depending on your environment, so you may receive an error (this can be safely ignored).

### Development Mode

To enable the Nancy development interface, you can use `--dev`. More details will be printed, including a generated key, when you launch OmegaGraf. By navigating to `/_Nancy` and entering your key, you should be able to obtain and change details about the web server, as well as observe requests. Additionally, the `--dev` trace disables Production Mode, meaning that the server will reply with the full stack trace if there are any unhandled exceptions.

### Additional Options

You can use `--help` to list all possible arguments, as well as a short description for each.
