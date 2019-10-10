using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace OmegaGraf.Compose
{
    public class Docker
    {
        public DockerClient DockerClient { get; }
        private string hostBind;
        private string dockerURI
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    this.hostBind = "/var/docker/telegraf";
                    return "unix:/var/run/docker.sock";
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    this.hostBind = "C:/docker/telegraf";
                    return "npipe://./pipe/docker_engine";
                }

                throw new System.NotSupportedException();
            }
        }

        public Docker() => this.DockerClient =
            new DockerClientConfiguration(new Uri(this.dockerURI))
                .CreateClient();

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

        public async Task<string> CreateContainer(string image)
        {
            var response = await this.DockerClient.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Image = image,
                    ExposedPorts = new Dictionary<string, EmptyStruct>
                    {
                        { "8083", default(EmptyStruct) },
                        { "8086", default(EmptyStruct) }
                    },
                    HostConfig = new HostConfig
                    {
                        Binds = new List<string>()
                        {
                            this.hostBind + ":/etc/telegraf"
                        },
                        PortBindings = new Dictionary<string, IList<PortBinding> >
                        {
                            {
                                "8083",
                                new List<PortBinding>
                                {
                                    new PortBinding { HostPort = "8083" }
                                }
                            },
                            {
                                "8086",
                                new List<PortBinding>
                                {
                                    new PortBinding { HostPort = "8086" }
                                }
                            }
                        },
                        PublishAllPorts = true
                    }
                }
            );

            return response.ID;
        }

        public async Task StartContainer(string id) =>
            await this.DockerClient.Containers.StartContainerAsync(
                id,
                new ContainerStartParameters()
            );

        public async Task DisposeAsync(string id)
        {
            if (id != null)
            {
                await this.DockerClient.Containers.KillContainerAsync(
                    id,
                    new ContainerKillParameters()
                );
            }
        }
    }
}