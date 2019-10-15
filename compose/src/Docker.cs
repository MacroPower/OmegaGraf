using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace OmegaGraf.Compose
{
    public class Docker : IDisposable
    {
        public DockerClient DockerClient { get; }
        private string DockerURI
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "unix:/var/run/docker.sock";
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "npipe://./pipe/docker_engine";
                }

                throw new NotSupportedException();
            }
        }

        public Docker()
        {
            this.DockerClient = new DockerClientConfiguration(new Uri(this.DockerURI))
                                .CreateClient();
        }

        public async Task PullImage(string image, string tag) =>
            await this.DockerClient.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = image,
                    Tag = tag
                },
                new AuthConfig(),
                new Progress<JSONMessage>()
            );

        /// <summary>
        /// Create the container and return the ID.
        /// Container will be STOPPED.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="ports">host = container</param>
        /// <param name="binds">host : container</param>
        /// <returns>Container UUID</returns>
        public async Task<string> CreateContainer(
            string image,
            List<int> ports,
            Dictionary<string, string> binds,
            List<string> cmd = null
        ){
            foreach (var b in binds)
            {
                System.IO.Directory.CreateDirectory(b.Key);
            }

            Dictionary<string, IList<PortBinding>> portBinds = (
                from port in ports
                from protocol in new string[] {
                    "tcp", "udp"
                }
                select new KeyValuePair<string, IList<PortBinding>>
                (
                    port + "/" + protocol,
                    new List<PortBinding>
                    {
                        new PortBinding
                        {
                            HostIP = "0.0.0.0",
                            HostPort = port.ToString()
                        }
                    }
                )).ToDictionary(i => i.Key, i => i.Value);

            var parameters = new CreateContainerParameters
            {
                Image = image,
                ExposedPorts = ports.ToDictionary(x => x.ToString(), x => default(EmptyStruct)),
                HostConfig = new HostConfig
                {
                    Binds = binds.Select(x => x.Key + ":" + x.Value).ToList(),
                    PortBindings = portBinds,
                    PublishAllPorts = false
                }
            };

            if (cmd != null && cmd.Count > 0)
            {
                parameters.Cmd = cmd;
            }

            var response = await this.DockerClient.Containers.CreateContainerAsync(parameters);

            return response.ID;
        }

        public async Task StartContainer(string id) =>
            await this.DockerClient.Containers.StartContainerAsync(
                id,
                new ContainerStartParameters()
            );

        public async Task KillContainer(string id)
        {
            if (id != null)
            {
                await this.DockerClient.Containers.KillContainerAsync(
                    id,
                    new ContainerKillParameters()
                );
            }
        }

        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                this.DockerClient.Dispose();
            }

            this.disposed = true;
        }
    }
}