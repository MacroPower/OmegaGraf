using System.Collections.Generic;
using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using System.Linq;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture]
    public class DeployTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var docker = new Docker();
            var network = docker.CreateNetwork().Result;

            if (network == null)
            {
                TestContext.Out.WriteLine("Network already exists.");
            }
        }

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
    }
}