using Nett;
using SharpYaml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmegaGraf.Compose.MetaData
{
    public class Input<T>
    {
        public BuildConfiguration BuildConfiguration { get; set; }
        public Config<T>[] Config { get; set; }
    }

    public class BuildConfiguration
    {
        public string Image { get; set; }
        public string Tag { get; set; }
        public List<int> Ports  { get; set; }
        public Dictionary<string, string> Binds { get; set; }
        public List<string> Parameters { get; set; }
    }

    public class Runner
    {
        private readonly Dictionary<string, string> configFile = new Dictionary<string, string>();

        public Runner AddYamlConfig<T>(params Config<T>[] config)
        {
            foreach (var c in config)
            {
                this.configFile.Add(
                    c.Path,
                    new Serializer(
                        new SerializerSettings()
                        {
                            SortKeyForMapping = false,
                            EmitAlias = false
                        }
                    ).Serialize(config, typeof(T))
                );
            }

            return this;
        }

        public Runner AddTomlConfig<T>(T config, string path)
        {
            var text = Toml.WriteString(config);

            this.configFile.Add(path, text);

            return this;
        }

        private void WriteConfig()
        {
            foreach (var c in configFile)
            {
                System.IO.File.WriteAllText(c.Key, c.Value);
            }
        }

        public string Build(BuildConfiguration config)
        {
            var buildTask = Task.Run(
                async () =>
                {
                    var docker = new Docker();
                    await docker.PullImage(config.Image, config.Tag);

                    var id = await docker.CreateContainer(
                        config.Image, config.Ports, config.Binds, config.Parameters
                    );

                    this.WriteConfig();

                    await docker.StartContainer(id);

                    return id;
                }
            );

            var uuid = buildTask.Result;

            return uuid;
        }
    }
}