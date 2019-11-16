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
            var sims = docker.ListContainers().Result;

            foreach (var sim in sims)
            {
                if (sim.Image == "macropower/vcsim:latest")
                {
                    docker.StopContainer(sim.ID).Wait();
                    docker.RemoveContainer(sim.ID).Wait();
                }
            }

            var vcConfig = new BuildConfiguration()
            {
                Image = "macropower/vcsim",
                Tag = "latest",
                Ports = new List<int>(){ },
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
            };

            var runner = new Runner();

            var uuid = runner.Build(vcConfig);

            vcConfig.Name = "vcsim2";

            uuid += "," + runner.Build(vcConfig);

            TestContext.Out.WriteLine(uuid);
        }

        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var config = Example.Telegraf.Config[0];

            var uuid = runner.AddTomlConfig(x => x.LowerCase, config).Build(Example.Telegraf.BuildConfiguration);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}