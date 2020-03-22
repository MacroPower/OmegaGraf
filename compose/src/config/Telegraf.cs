using System.Collections.Generic;
using Nett;

namespace OmegaGraf.Compose.Config.Telegraf
{
    /// <summary>
    /// Represents the telegraf.conf file
    /// </summary>
    public class Telegraf
    {
        /// <summary>
        /// Configuration for telegraf agent
        /// </summary>
        public Agent Agent { get; set; }
        public Outputs Outputs { get; set; }
        public Inputs Inputs { get; set; }
    }

    public class Agent
    {
        /// <summary>
        /// Default data collection interval for all inputs
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// Rounds collection interval to 'interval'
        /// ie, if interval="10s" then always collect on :00, :10, :20, etc.
        /// </summary>
        [TomlMember(Key = "round_interval")]
        public bool RoundInterval { get; set; }

        /// <summary>
        /// Telegraf will send metrics to outputs in batches of at most metric_batch_size metrics.
        /// This controls the size of writes that Telegraf sends to output plugins.
        /// </summary>
        [TomlMember(Key = "metric_batch_size")]
        public int MetricBatchSize { get; set; }

        /// <summary>
        /// Maximum number of unwritten metrics per output.
        /// </summary>
        [TomlMember(Key = "metric_buffer_limit")]
        public int MetricBufferLimit { get; set; }

        /// <summary>
        /// Collection jitter is used to jitter the collection by a random amount.
        /// Each plugin will sleep for a random time within jitter before collecting.
        /// This can be used to avoid many plugins querying things like sysfs at the
        /// same time, which can have a measurable effect on the system.
        /// </summary>
        [TomlMember(Key = "collection_jitter")]
        public string CollectionJitter { get; set; }

        /// <summary>
        /// Default flushing interval for all outputs. Maximum flush_interval will be
        /// flush_interval + flush_jitter
        /// </summary>
        [TomlMember(Key = "flush_interval")]
        public string FlushInterval { get; set; }

        /// <summary>
        /// Jitter the flush interval by a random amount. This is primarily to avoid
        /// large write spikes for users running a large number of telegraf instances.
        /// ie, a jitter of 5s and interval 10s means flushes will happen every 10-15s
        /// </summary>
        [TomlMember(Key = "flush_jitter")]
        public string FlushJitter { get; set; }

        /// <summary>
        /// By default or when set to "0s", precision will be set to the same
        /// timestamp order as the collection interval, with the maximum being 1s.
        ///   ie, when interval = "10s", precision will be "1s"
        ///       when interval = "250ms", precision will be "1ms"
        /// Precision will NOT be used for service inputs. It is up to each individual
        /// service input to set the timestamp at the appropriate precision.
        /// Valid time units are "ns", "us" (or "µs"), "ms", "s".
        /// </summary>
        public string Precision { get; set; }

        /// <summary>
        /// Override default hostname, if empty use os.Hostname()
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// If set to true, do no set the "host" tag in the telegraf agent.
        /// </summary>
        [TomlMember(Key = "omit_hostname")]
        public bool OmitHostname { get; set; }
    }

    public class Inputs
    {
        public IEnumerable<Internal> Internal { get; set; }
        public IEnumerable<VSphere> VSphere { get; set; }
    }

    public class Outputs
    {
        /// <summary>
        /// Configuration for the Prometheus client to spawn
        /// </summary>
        [TomlMember(Key = "prometheus_client")]
        public IEnumerable<PrometheusClient> PrometheusClient { get; set; }
    }

    public class PrometheusClient
    {
        /// <summary>
        /// Address to listen on
        /// </summary>
        /// <value>:10000</value>
        public string Listen { get; set; }

        /// <summary>
        /// Send string metrics as Prometheus labels.
        /// Unless set to false all string metrics will be sent as labels.
        /// </summary>
        [TomlMember(Key = "string_as_label")]
        public bool StringAsLabel { get; set; }

        /// <summary>
        /// Expiration interval for each metric. 0 == no expiration
        /// </summary>
        [TomlMember(Key = "expiration_interval")]
        public string ExpirationInterval { get; set; }

        /// <summary>
        /// Path to publish the metrics on.
        /// </summary>
        /// <value>/metrics</value>
        public string Path { get; set; }
    }

    public class Internal
    {
        /// <summary>
        /// Collect telegraf memory stats.
        /// </summary>
        [TomlMember(Key = "collect_memstats")]
        public bool CollectMemstats { get; set; }
    }

    public class VSphere
    {
        public List<string> VCenters { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [TomlMember(Key = "ip_addresses")]
        public List<string> IPAddresses { get; set; }
        [TomlMember(Key = "use_int_samples")]
        public bool IntSamples { get; set; }
        [TomlMember(Key = "insecure_skip_verify")]
        public bool InsecureSkipVerify { get; set; }
        [TomlMember(Key = "force_discover_on_init")]
        public bool ForceDiscover { get; set; }
        public string Interval { get; set; }
        [TomlMember(Key = "datastore_metric_exclude")]
        public List<string> DatastoreMetricExclude { get; set; }
        [TomlMember(Key = "cluster_metric_exclude")]
        public List<string> ClusterMetricExclude { get; set; }
        [TomlMember(Key = "datacenter_metric_exclude")]
        public List<string> DatacenterMetricExclude { get; set; }
        [TomlMember(Key = "host_metric_exclude")]
        public List<string> HostMetricExclude { get; set; }
        [TomlMember(Key = "vm_metric_exclude")]
        public List<string> VMMetricExclude { get; set; }
        [TomlMember(Key = "max_query_metrics")]
        public int MaxQueryMetrics { get; set; }

        [TomlMember(Key = "collect_concurrency")]
        public int CollectConcurrency { get; set; }
        [TomlMember(Key = "discover_concurrency")]
        public int DiscoverConcurrency { get; set; }
    }
}