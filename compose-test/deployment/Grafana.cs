using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using OmegaGraf.Compose.Config.Grafana;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture, NonParallelizable]
    [Category("Deployment")]
    public class Grafana : DeployTests
    {
        [Test, Order(1)]
        public void Deploy()
        {
            var runner = new Runner();

            var uuid = runner.Build(Example.Grafana.BuildConfiguration);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Thread.Sleep(15000); //lazy way to make sure the API is online

            Assert.Pass();
        }

        [Test, Order(2)]
        public void CreateDataSource()
        {
            var g = new Compose.Grafana();

            try
            {
                var dataSource = new DataSource()
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
                };

                g.AddDataSource(dataSource).Wait();

                g.Dispose();
            }
            catch (Exception e)
            {
                TestContext.Out.WriteLine("Error - disposing objects.");
                g.Dispose();
                throw e;
            }

            Assert.Pass();
        }

        [Test, Order(2)]
        public void CreateDashboard()
        {
            var g = new Compose.Grafana();

            try
            {
                var dash = JsonConvert.DeserializeObject(File.ReadAllText("assets\\Dashboard.json"));

                g.AddDashboard(dash).Wait();

                g.Dispose();
            }
            catch (Exception e)
            {
                TestContext.Out.WriteLine("Error - disposing objects.");
                g.Dispose();
                throw e;
            }

            Assert.Pass();
        }
    }
}