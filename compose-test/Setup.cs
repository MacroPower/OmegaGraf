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
    }
}