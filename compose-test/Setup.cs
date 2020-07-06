using NUnit.Framework;
using OmegaGraf.Compose.MetaData;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture]
    public class DeployTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            SystemData.SetRoot("/docker");

            var docker = new Docker();
            var network = docker.CreateNetwork().Result;

            if (network == null)
            {
                TestContext.Out.WriteLine("Network already exists.");
            }
        }
    }
}
