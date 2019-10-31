using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmegaGraf.Compose.Tests.Destroyer
{
    [TestFixture]
    public class Destroy
    {
        [Test]
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