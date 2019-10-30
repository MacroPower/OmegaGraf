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
                    Image = "macropower/vcsim",
                    Tag = "latest",
                    Ports = new List<int>(){ 8989 },
                    Binds = new Dictionary<string, string>(){ },
                    Parameters = new List<string>()
                    {
                        "--clusters", "2",
                        "--data-centers", "1",
                        "--data-stores", "2",
                        "--hosts", "5",
                        "--resource-pools", "1",
                        "--standalone-host", "10",
                        "--virtual-machines", "20",
                    }
                });

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}