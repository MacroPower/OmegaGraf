using System;
using NUnit.Framework;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture]
    [Category("Deployment")]
    public class Prometheus
    {
        private const string Image = "prom/prometheus";

        [OneTimeSetUp]
        public void Cleanup()
        {
            var docker = new Docker();
            var containers = docker.ListContainers().Result;

            foreach (var container in containers)
            {
                if (container.Image.Contains(Image))
                {
                    docker.StopContainer(container.ID).Wait();
                    docker.RemoveContainer(container.ID).Wait();
                }
            }
        }

        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var config = Defaults.Prometheus.Config[0];

            var bc = Defaults.Prometheus.BuildInput
                .ToBuildConfiguration(Image);

            var uuid = runner.AddYamlConfig(config).Build(bc);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}
