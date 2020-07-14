using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OmegaGraf.Compose
{
    public static class TranslateConfig
    {
        public static BuildConfiguration ToBuildConfiguration(this BuildConfigurationInput input, string image) =>
            new BuildConfiguration()
            {
                Name       = input.Name,
                Image      = image,
                Tag        = input.Tag,
                Ports      = input.Ports,
                Binds      = input.Binds,
                Parameters = input.Parameters
            };
    }

    public class BuildConfiguration
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Tag { get; set; }
        public Dictionary<int, int> Ports { get; set; }
        private Dictionary<string, string> _binds;
        public Dictionary<string, string> Binds
        {
            get => this._binds;
            set => this._binds =
                value.ToDictionary(
                    d => Path.Join(AppPath.GetRoot(), d.Key),
                    d => d.Value
                );
        }
        public List<string> Parameters { get; set; }
    }
}
