# Parameters

OmegaGraf has several parameters designed for more advanced users.

## Reconfigure & Redeploy

To make changes to any container created by OmegaGraf, you can run with the `--reset` or `--overwrite` switch. `--reset` will remove all OmegaGraf containers at start, `--overwrite` will remove containers as you redeploy them. Neither switch will delete any data (e.g. time series data, custom dashboards/datasources). If you set either one of these switches you should be able to re-run the deployment process without issue. Note that currently, we cannot always reconfigure Grafana depending on your environment, so you may receive an error (this can be safely ignored).

## Development Mode

To enable the Nancy development interface, you can use `--dev`. More details will be printed, including a generated key, when you launch OmegaGraf. By navigating to `/_Nancy` and entering your key, you should be able to obtain and change details about the web server, as well as observe requests. Additionally, the `--dev` trace disables Production Mode, meaning that the server will reply with the full stack trace if there are any unhandled exceptions.

## Additional Options

You can use `--help` to list all possible arguments, as well as a short description for each.
