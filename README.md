# og-compose

This web service builds and configures docker containers.

Currently supported containers:
- Prometheus

For example, see the following example request body:

```
{
  "BuildConfiguration": {
    "Image": "prom/prometheus",
    "Tag": "latest",
    "Ports": [
      9090
    ],
    "Binds": {
      "C:/docker/prometheus/config": "/etc/prometheus",
      "C:/docker/prometheus/data": "/prometheus"
    },
    "Parameters": [
      "--config.file=/etc/prometheus/prometheus.yml",
      "--storage.tsdb.path=/prometheus"
    ]
  },
  "Config": [
    {
      "Path": "C:/docker/prometheus/config/prometheus.yml",
      "Data": {
        "Global": {
          "ScrapeInterval": "5s"
        },
        "ScrapeConfigs": [
          {
            "JobName": "prometheus",
            "ScrapeInterval": "5s",
            "StaticConfigs": [
              {
                "Targets": [
                  "localhost:9090"
                ]
              }
            ]
          }
        ]
      }
    }
  ]
}
```

Comprehensive documentation can be found at `/api-docs`.
