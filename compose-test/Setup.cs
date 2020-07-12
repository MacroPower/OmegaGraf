using System;
using System.IO;
using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
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

            if (!Globals.Config.Environment.IsWindows)
            {
                chmod(tempDirectory, P0777);
            }

            SystemData.SetRoot(tempDirectory);

            var docker = new Docker();

            docker.RemoveAllContainers().Wait();

            var network = docker.CreateNetwork().Result;

            if (network == null)
            {
                TestContext.Out.WriteLine("Network already exists.");
            }
        }
    }
}
