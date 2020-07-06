using Nett;
using NLog;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaGraf.Compose.MetaData
{
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
            set => this._binds = value.ToDictionary(d => Path.Join(SystemData.GetRoot(), d.Key), d => d.Value);
        }
        public List<string> Parameters { get; set; }
    }

    public class Runner
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, string> _configFile = new Dictionary<string, string>();

        public Runner AddYamlConfig<T>(params Config<T>[] config)
        {
            foreach (var c in config)
            {
                this._configFile.Add(
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

                this._configFile.Add(c.Path, text);
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

                this._configFile.Add(c.Path, text);
            }

            return this;
        }

        private void WriteConfig()
        {
            foreach (var c in this._configFile)
            {
                File.WriteAllText(Path.Join(SystemData.GetRoot(), c.Key), c.Value);
            }
        }

        private async Task<string> BuildFromConfigAsync(BuildConfiguration config)
        {
            try
            {
                var docker = new Docker();
                await docker.PullImage(config.Image, config.Tag);

                var id = await docker.CreateContainer(
                    image: config.Image,
                    ports: config.Ports,
                    binds: config.Binds,
                    name: config.Name,
                    tag: config.Tag,
                    cmd: config.Parameters
                );

                this.WriteConfig();

                await docker.StartContainer(id);

                return id;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to create Docker container");
                throw;
            }
        }

        public string Build(BuildConfiguration config)
        {
            return this.BuildFromConfigAsync(config).Result;
        }
    }
}
