using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using System;

namespace OmegaGraf.Compose.Tests
{
    public class Grafana
    {
        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var uuid = runner.Build(Example.Grafana.BuildConfiguration);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}