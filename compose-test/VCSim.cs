using System;
using System.Collections.Generic;
using NUnit.Framework;
using OmegaGraf.Compose.MetaData;

namespace OmegaGraf.Compose.Tests
{
    public class VCSim
    {
        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var uuid = runner.Build(
                new BuildConfiguration()
                {
                    Image = "m451/vcsim",
                    Tag = "latest",
                    Ports = new List<int>(){ 8989 },
                    Binds = new Dictionary<string, string>(){ }
                });

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}