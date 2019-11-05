# og-compose

This web service builds and configures docker containers.

Currently supported containers:
- Prometheus
- Telegraf
- Grafana
- [VCSim](https://github.com/OmegaGraf/docker-vcsim)

Requests tend to consist of two parts, a `BuildConfiguration` and a `Config`.

`BuildConfiguration` sets container parameters, such as name, image, bindings, etc.
`Config` is a combination of data to serialize, and the path where the data should be written.
There are a few additional calls for configuration, and they do not follow any standards.

```
{
  "BuildConfiguration": {
    "Image": "macropower/name",
    "Tag": "latest",
    "Ports": [],
    "Binds": {},
    "Parameters": []
  },
  "Config": [
    {
      "Path": "C:/docker/...",
      "Data": { }
    }
  ]
}
```

Comprehensive documentation can be found at `/api-docs`.
