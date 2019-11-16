using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using OmegaGraf.Compose.Config.Grafana;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Flurl.Http;
using System.Threading.Tasks;

namespace OmegaGraf.Compose.Tests.Builder
{
    [TestFixture, NonParallelizable]
    [Category("Deployment")]
    public class Grafana : DeployTests
    {
        private static readonly int port = Example.Grafana.BuildConfiguration.Ports.First();
        private static async Task<bool> IsOnline()
        {
            try
            {
                var response = await ("http://localhost:" + port)
                               .GetAsync();

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        [Test, Order(1)]
        public void Deploy()
        {
            var runner = new Runner();

            var uuid = runner.Build(Example.Grafana.BuildConfiguration);

            TestContext.Out.WriteLine("Container: " + uuid);
            TestContext.Out.WriteLine("Port: " + port);
            TestContext.Out.WriteLine("Mode: " + Example.Mode);

            var wait = Is.True.After(30000, 2000);

            Assert.That(() => IsOnline().Result, wait);
        }

        [Test, Order(2)]
        public void CreateDataSource()
        {
            var g = new Compose.Grafana("http://localhost:" + port);

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
            var g = new Compose.Grafana("http://localhost:" + port);

            try
            {
                var dash = JsonConvert.DeserializeObject(File.ReadAllText("assets/Dashboard.json"));

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