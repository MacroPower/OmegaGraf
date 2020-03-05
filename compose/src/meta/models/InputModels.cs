using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OmegaGraf.Compose.MetaData
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

    public static class TranslateConfig
    {
        public static BuildConfiguration ToBuildConfiguration(this BuildConfigurationInput input, string image)
        {
            // Append the absolute path to the input path, using either the current directory or the user specified directory.
            // var absolutePathBinds = input.Binds.ToDictionary(d => Path.Join(SystemData.Root, d.Key), d => d.Value);

            return new BuildConfiguration()
                   {
                       Name = input.Name,
                       Image = image,
                       Tag = input.Tag,
                       Ports = input.Ports,
                       Binds = input.Binds,
                       Parameters = input.Parameters
                   };
        }
    }
}