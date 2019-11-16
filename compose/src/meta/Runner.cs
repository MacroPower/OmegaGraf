using Nett;
using SharpYaml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace OmegaGraf.Compose.MetaData
{
    public class Input<T>
    {
        public BuildConfiguration BuildConfiguration { get; set; }
        public Config<T>[] Config { get; set; }
    }

    public class Input
    {
        public BuildConfiguration BuildConfiguration { get; set; }
    }

    public class BuildConfiguration
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Tag { get; set; }
        public List<int> Ports { get; set; }
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
                    ).Serialize(c.Data, typeof(T))
                );
            }

            return this;
        }

        public Runner AddTomlConfig<T>(params Config<T>[] config)
        {
            foreach (var c in config)
            {
                var text = Toml.WriteString(c.Data);

                this.configFile.Add(c.Path, text);
            }

            return this;
        }

        public Runner AddTomlConfig<T>(System.Func<KeyGenerators, IKeyGenerator> keyGenerator, params Config<T>[] config)
        {
            var tomlSettings = TomlSettings.Create(
                s => s.ConfigurePropertyMapping(
                    m => m.UseKeyGenerator(keyGenerator)));

            foreach (var c in config)
            {
                var text = Toml.WriteString(c.Data, tomlSettings);

                this.configFile.Add(c.Path, text);
            }

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

                    var ports = config.Ports.ToDictionary(x => x, x => x);

                    var id = await docker.CreateContainer(
                        image: config.Image,
                        ports: ports,
                        binds: config.Binds,
                        name: config.Name,
                        tag: config.Tag,
                        cmd: config.Parameters
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