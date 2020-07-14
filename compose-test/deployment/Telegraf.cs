using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture]
    [Category("Deployment")]
    public class Telegraf
    {
        private const string Image = "telegraf";

        [OneTimeSetUp]
        public void DeployVCSim()
        {
            var docker = new Docker();

            docker.RemoveContainers("og-vcsim").Wait();
            docker.RemoveContainers("og-vcsim2").Wait();
            docker.RemoveContainers("og-telegraf").Wait();

            var vcConfig = new BuildConfiguration()
            {
                Image = "macropower/vcsim",
                Tag = "latest",
                Ports = new Dictionary<int, int>() { },
                Binds = new Dictionary<string, string>() { },
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

            var config = Defaults.Telegraf.Config[0];

            var bc = Defaults.Telegraf.BuildInput
                .ToBuildConfiguration(Image);

            var uuid = runner.AddTomlConfig(x => x.LowerCase, config).Build(bc);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}
