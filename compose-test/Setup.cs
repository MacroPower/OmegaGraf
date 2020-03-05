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
            SystemData.Root = "/docker";

            var docker = new Docker();
            var network = docker.CreateNetwork().Result;

            if (network == null)
            {
                TestContext.Out.WriteLine("Network already exists.");
            }
        }
    }
}