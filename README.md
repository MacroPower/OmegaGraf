<p align="center">
  <img src="docs/branding/logo_128.png">
</p>

# OmegaGraf

OmegaGraf is an entirely open-source and containerized solution, configured and deployed through a simple web interface. In minutes, OmegaGraf provides dynamic dashboards, granular time series data, environment aggregation, alerting, and much more.

## Requirements

[Click Here](https://github.com/OmegaGraf/install) for automated installers.

- Linux (prod/dev) or Windows OS (dev)
- Docker (prod/dev) or Docker Desktop (dev)
- OpenSSL (prod)

## Using OmegaGraf

1. Run OmegaGraf application
2. Visit printed URL and enter secure code
3. Click any option
4. Check 'Use simulation' and enter desired # of sims
5. Click 'Deploy'
6. Click 'Confirm'
7. Wait for deployment to complete
8. Visit {hostname}:3000, enter admin/admin
9. Done!

## Create a release

```
run ./build.ps1
--> build/{env}/
```
