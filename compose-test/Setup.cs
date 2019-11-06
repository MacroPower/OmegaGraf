using System.Collections.Generic;
using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using System.Linq;
using System.Threading.Tasks;

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

        [OneTimeTearDown]
        public void KillContainers()
        {
            var docker = new Docker();
            var containers = docker.ListContainers().Result;
            var tasks = new List<Task>();

            foreach (var container in containers)
            {
                foreach (var name in container.Names)
                {
                    if (name.StartsWith("/og-"))
                    {
                        TestContext.Out
                            .WriteLine("Removing " + name + ", " + container.ID);

                        tasks.Add(
                            Task.Run(async () => {
                                     await docker.StopContainer(container.ID);
                                     await docker.RemoveContainer(container.ID);
                                 }));
                    }
                }
            }

            Task t = Task.WhenAll(tasks);

            t.Wait();
        }
    }
}