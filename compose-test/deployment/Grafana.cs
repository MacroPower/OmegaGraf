using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using OmegaGraf.Compose.Config.Grafana;
using System;
using System.IO;
using Newtonsoft.Json;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture]
    [Category("Deployment")]
    public class Grafana : DeployTests
    {
        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var uuid = runner.Build(Example.Grafana.BuildConfiguration);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }

        [Test]
        public void Config()
        {
            var g = new Compose.Grafana();

            g.AddDataSource(
                new DataSource()
                {
                    ID = 1,
                    OrgID = 1,
                    Name = "og-prometheus",
                    Type = "prometheus",
                    Access = "proxy",
                    URL = "http://og-prometheus:9090",
                    Password = "",
                    User = "",
                    Database = "",
                    BasicAuth = false,
                    BasicAuthUser = "",
                    BasicAuthPassword = "",
                    IsDefault = true,
                    JsonData = null
                }).Wait();

            var dash = JsonConvert.DeserializeObject(File.ReadAllText("assets\\Dashboard.json"));

            g.AddDashboard(dash).Wait();
        }
    }
}