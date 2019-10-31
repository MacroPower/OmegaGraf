using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture]
    [Category("Deployment")]
    public class Telegraf : DeployTests
    {
        [OneTimeSetUp]
        public void DeployVCSim()
        {
            var docker = new Docker();
            var sims = docker.ListContainers().Result.Where(x => x.Names.Contains("/og-vcsim"));

            foreach (var sim in sims)
            {
                docker.StopContainer(sim.ID).Wait();
                docker.RemoveContainer(sim.ID).Wait();
            }

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
                        "--standalone-host", "0",
                        "--virtual-machines", "20",
                    }
                });

            TestContext.Out.WriteLine("docker container logs " + uuid);
            TestContext.Out.WriteLine("docker container inspect " + uuid);
            TestContext.Out.WriteLine("docker container stop " + uuid);
            TestContext.Out.WriteLine("docker container rm " + uuid);
        }

        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var config = new Config<Config.Telegraf.Telegraf>()
            {
                Path = @"C:\docker\telegraf\telegraf.conf",
                Data = Example.Telegraf.Config[0].Data
            };

            var uuid = runner.AddTomlConfig(x => x.LowerCase, config).Build(Example.Telegraf.BuildConfiguration);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}