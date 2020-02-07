# OmegaGraf

OmegaGraf is an entirely open-source and containerized solution, configured and deployed through a simple web interface. In minutes, OmegaGraf provides dynamic dashboards, granular time series data, environment aggregation, alerting, and much more.

## Create a release

```
cd ui
npm install
npm run build
cd ..
dotnet build compose/compose.csproj /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary
dotnet publish compose/compose.csproj -c=Release -r=linux-x64 /property:GenerateFullPaths=true /property:PublishSingleFile=true /property:PublishTrimmed=true /consoleloggerparameters:NoSummary
```