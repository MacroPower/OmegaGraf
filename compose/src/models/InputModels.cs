using System.Collections.Generic;

namespace OmegaGraf.Compose
{
    public class Input<T>
    {
        public BuildConfigurationInput BuildInput { get; set; }
        public Config<T>[] Config { get; set; }
    }

    public class Input
    {
        public BuildConfigurationInput BuildInput { get; set; }
    }

    public class BuildConfigurationInput
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public Dictionary<int, int> Ports { get; set; }
        public Dictionary<string, string> Binds { get; set; }
        public List<string> Parameters { get; set; }
    }

    public class GrafanaInput
    {
        public int Port { get; set; }
    }
}
