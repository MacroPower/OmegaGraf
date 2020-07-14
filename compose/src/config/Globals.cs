using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace OmegaGraf.Compose
{
    public class Runtime
    {
        public string Framework => RuntimeInformation.FrameworkDescription;
        public string Architecture => RuntimeInformation.OSArchitecture.ToString();
        public string Description => RuntimeInformation.OSDescription;
        public string ProcessArchitecture => RuntimeInformation.ProcessArchitecture.ToString();
        public string Platform => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux"
                                : RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows"
                                : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "Darwin"
                                : "Unknown";

        [JsonIgnore]
        public bool IsLinux { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        [JsonIgnore]
        public bool IsWindows { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        [JsonIgnore]
        public bool IsDarwin { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }

    public class GlobalConfig
    {
        public string Hostname { get; set; } = System.Net.Dns.GetHostName();
        public Runtime Environment { get; set; } = new Runtime();
        public string Version { get; set; } = "Unknown";
        public bool Development { get; set; } = false;
        public bool Overwrite { get; set; } = false;
    }

    public static class Globals
    {
        public static GlobalConfig Config { get; set; } = new GlobalConfig();

        public static GlobalConfig Clone() => new GlobalConfig()
        {
            Hostname    = Config.Hostname,
            Environment = Config.Environment,
            Version     = Config.Version,
            Development = Config.Development,
            Overwrite   = Config.Overwrite,
        };
    }
}
