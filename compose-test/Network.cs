using System.Threading.Tasks;
using NUnit.Framework;

namespace OmegaGraf.Compose.Tests
{
    public class Network
    {
        [Test]
        public void Deploy()
        {
            var buildTask = Task.Run(
                async () =>
                {
                    var docker = new Docker();
                    await docker.CreateNetwork();
                }
            );

            buildTask.Wait();

            Assert.Pass();
        }
    }
}