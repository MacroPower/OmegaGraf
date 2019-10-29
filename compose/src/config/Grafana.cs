using Newtonsoft.Json;

namespace OmegaGraf.Compose.Config.Grafana
{
    public class Token
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class DataSource
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("orgId")]
        public int OrgID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Access { get; set; }
        [JsonProperty("url")]
        public string URL { get; set; }
        public string Password { get; set; }
        public string User { get; set; }
        public string Database { get; set; }
        public bool BasicAuth { get; set; }
        public string BasicAuthUser { get; set; }
        public string BasicAuthPassword { get; set; }
        public bool IsDefault { get; set; }
        public dynamic JsonData { get; set; }
    }
}