using System;
using System.IO;
using NUnit.Framework;

using static OmegaGraf.Compose.Unix;

namespace OmegaGraf.Compose.Tests.Builder
{
    [SetUpFixture]
    public class DeployTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            var tempName = "OmegaGraf-" + Guid.NewGuid().ToString();
            var tempDirectory = Path.Combine(Path.GetTempPath(), tempName);
            Directory.CreateDirectory(tempDirectory);

            Chmod(tempDirectory, P0777);

            AppPath.SetRoot(tempDirectory);

            var docker = new Docker();

            var network = docker.CreateNetwork().Result;

            if (network == null)
            {
                TestContext.Out.WriteLine("Network already exists.");
            }
        }
    }
}
