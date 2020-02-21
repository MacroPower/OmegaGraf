using Docker.DotNet;
using Docker.DotNet.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OmegaGraf.Compose
{
    public class Docker
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
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
            var uri = new Uri(this.DockerURI);

            using (var config = new DockerClientConfiguration(uri))
            {
                this.DockerClient = config.CreateClient();
            }
        }

        public Task PullImage(string image, string tag) =>
            this.DockerClient.Images.CreateImageAsync(
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
            Dictionary<int, int> ports,
            Dictionary<string, string> binds,
            string name = "",
            string tag = "latest",
            List<string> cmd = null
        ){
            await CreateNetwork();
            
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
                    port.Value + "/" + protocol,
                    new List<PortBinding>
                    {
                        new PortBinding
                        {
                            HostIP = "0.0.0.0",
                            HostPort = port.Key.ToString()
                        }
                    }
                )).ToDictionary(i => i.Key, i => i.Value);

            var exposedPorts = portBinds.ToDictionary(x => x.Key, x => default(EmptyStruct));
            var hostBinds    = binds.Select(x => x.Key + ":" + x.Value).ToList();
            var hostname     = "og-" + (string.IsNullOrWhiteSpace(name) ? Regex.Replace(image, @"^.*/", "") : name);

            var parameters = new CreateContainerParameters
            {
                Name = hostname,
                Hostname = hostname,
                NetworkingConfig = new NetworkingConfig()
                {
                    EndpointsConfig = new Dictionary<string, EndpointSettings>()
                    {
                        {
                            "og-network",
                            new EndpointSettings()
                            {
                                NetworkID = "og-network",
                                Aliases = new string[]
                                {
                                    hostname
                                }
                            }
                        }
                    }
                },
                Image = image + ":" + tag,
                ExposedPorts = exposedPorts,
                HostConfig = new HostConfig
                {
                    Binds = hostBinds,
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

        public Task StartContainer(string id) =>
            this.DockerClient.Containers.StartContainerAsync(
                id,
                new ContainerStartParameters()
            );

        public Task<IList<ContainerListResponse>> ListContainers() =>
            this.DockerClient.Containers.ListContainersAsync(
                new ContainersListParameters()
                {
                    All = true
                }
            );

        public Task StopContainer(string id) =>
            this.DockerClient.Containers
                .StopContainerAsync(
                    id,
                    new ContainerStopParameters()
                );

        public Task RemoveContainer(string id) =>
            this.DockerClient.Containers
                .RemoveContainerAsync(
                    id,
                    new ContainerRemoveParameters()
                    {
                        RemoveVolumes = false,
                        RemoveLinks = false,
                        Force = false
                    }
                );
        
        public Task RemoveAllContainers()
        {
            var containers = ListContainers().Result;
            var tasks = new List<Task>();

            foreach (var container in containers)
            {
                foreach (var name in container.Names)
                {
                    if (name.StartsWith("/og-"))
                    {
                        logger.Info("Removing " + name + ", " + container.ID);

                        tasks.Add(
                            Task.Run(async () => {
                                    await StopContainer(container.ID);
                                    await RemoveContainer(container.ID);
                                }));
                    }
                }
            }

            return Task.WhenAll(tasks);
        }

        public async Task<NetworksCreateResponse> CreateNetwork()
        {
            var name = "og-network";
            var networks = await this.DockerClient.Networks.ListNetworksAsync();

            if (networks.Any(x => x.Name == name))
                return null;

            return await this.DockerClient.Networks.CreateNetworkAsync(
                new NetworksCreateParameters()
                {
                    Name = name,
                    Driver = "bridge",
                    CheckDuplicate = true,
                    IPAM = new IPAM()
                    {
                        Driver = "default",
                        Config = new List<IPAMConfig>()
                        {
                            new IPAMConfig()
                            {
                                Subnet = "172.20.0.0/16",
                                IPRange = "172.20.10.0/24",
                                Gateway = "172.20.10.11"
                            }
                        }
                    }
                }
            );
        }

        ~Docker()
        {
            this.DockerClient.Dispose();
        }
    }
}