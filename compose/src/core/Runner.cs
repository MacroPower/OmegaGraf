using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nett;
using NLog;
using SharpYaml.Serialization;

using static OmegaGraf.Compose.Unix;

namespace OmegaGraf.Compose
{
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

        public Runner AddTomlConfig<T>(Func<KeyGenerators, IKeyGenerator> keyGenerator, params Config<T>[] config)
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

        public Runner AddConfig(params Config<string>[] config)
        {
            foreach (var c in config)
            {
                this._configFile.Add(c.Path, c.Data);
            }

            return this;
        }

        private void SetPermissionsRecursive(string dir)
        {
            chmod(dir, P0777);

            foreach (var file in Directory.GetFiles(dir))
            {
                chmod(file, P0666);
            }

            foreach (var d in Directory.GetDirectories(dir))
            {
                this.SetPermissionsRecursive(d);
            }
        }

        private void WriteConfig()
        {
            foreach (var c in this._configFile)
            {
                if (Globals.Config.Environment.IsWindows)
                {
                    var path = Path.Join(AppPath.GetRoot(), c.Key);
                    File.WriteAllText(path, c.Value);
                }
                else
                {
                    // Note: This entire block is a hack that exists due to Microsoft
                    //       refusing to fix System.IO's consistency for over 5 years.
                    //       This is why you don't have any friends, Microsoft.
                    //       dotnet/runtime/issues/13946

                    var root = AppPath.GetRoot();
                    var path = Path.Join(root, c.Key);

                    // Ensure that the root directory maintains the correct permissions
                    this.SetPermissionsRecursive(root);

                    // Write the file and set its permissions
                    File.WriteAllText(path, c.Value);
                    this.SetPermissionsRecursive(root);
                }

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
