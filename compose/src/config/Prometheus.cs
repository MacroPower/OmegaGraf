using System.Collections.Generic;
using SharpYaml;
using SharpYaml.Serialization;

namespace OmegaGraf.Compose.Config.Prometheus
{
    /// <summary>
    /// Represents the prometheus.yaml config file
    /// </summary>
    public class Prometheus
    {
        [YamlMember("global")]
        public Global Global { get; set; }

        [YamlMember("scrape_configs")]
        public List<ScrapeConfigs> ScrapeConfigs { get; set; }
    }

    public class Global
    {
        [YamlMember("scrape_interval")]
        public string ScrapeInterval { get; set; }
    }

    public class ScrapeConfigs
    {
        [YamlMember("job_name")]
        public string JobName { get; set; }

        [YamlMember("scrape_interval")]
        public string ScrapeInterval { get; set; }

        [YamlMember("static_configs")]
        public List<StaticConfigs> StaticConfigs { get; set; }
    }

    public class StaticConfigs
    {
        [YamlMember("targets")]
        [YamlStyle(YamlStyle.Flow)]
        public string[] Targets { get; set; }
    }
}
