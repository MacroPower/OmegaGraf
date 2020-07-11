# Grafana Auto-Config

In the last part of deployments, the Prometheus datasource is added to Grafana.

After this datasource is added, OmegaGraf will scan the dashboards/ directory for JSON files containing dashboard data.

If any are found, they will be added to the Grafana instance.

## Adding a Dashboard

Since this is all automatic, you only need to add your JSON to the dashboards folder.

In the scripts folder, there is a Powershell script that will automatically cleanup exports from Grafana.

If you run it after placing JSON files in the dashboards folder, it will format them in-place.

### Steps

- Export dashboard from Grafana to JSON file
- Copy JSON file to dashboards/
- `cd scripts`
- `./cleanupDashboards.ps1`
- `git commit && git push`
- Done! Your dashboard will be included with the next release.

### Jsonnet

It is possible to use Jsonnet with graffonetlibs. More documentation coming soon on this.

### Diagram

```text
+--------------------+    +--------------------+
|                    |    |                    |     .---------------.
| OmegaGraf UI / API |    |      Jsonnet       |--->(      make       )
|                    |    |                    |     `---------------'
+--------------------+    +--------------------+             |
           |                                                 |
           v                                                 |
+--------------+-----+                                       v
|              |     |                             * Dashboard1.json
|  OmegaGraf   | Bin |<----------Compile---------- * Dashboard2.json
|              |     |                             * ...
+--------------+-----+                                       ^
           |                                                 |
           v                                                 |
+--------------------+                                       |
|                    |                               .---------------.
|      Grafana       |- - - - - -Changes- - - - - ->(     Export      )
|                    |                               `---------------'
+--------------------+
```
