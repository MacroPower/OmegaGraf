using NUnit.Framework;
using System;
using OmegaGraf.Compose.MetaData;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture]
    [Category("Deployment")]
    public class Prometheus : DeployTests
    {
        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var config = Defaults.Prometheus.Config[0];

            var bc = Defaults.Prometheus.BuildInput.ToBuildConfiguration("prom/prometheus");

            var uuid = runner.AddYamlConfig(config).Build(bc);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}