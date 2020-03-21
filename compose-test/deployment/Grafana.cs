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
        private static readonly int port = Defaults.Grafana.BuildInput.Ports.First().Key;
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

            var bc = Defaults.Grafana.BuildInput.ToBuildConfiguration("grafana/grafana");

            var uuid = runner.Build(bc);

            TestContext.Out.WriteLine("Container: " + uuid);
            TestContext.Out.WriteLine("Port: " + port);
            TestContext.Out.WriteLine("Mode: " + SystemData.GetMode());

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
            var d = File.ReadAllText("grafana/dashboards/_Example.json");

            var g = new Compose.Grafana("http://localhost:" + port);

            try
            {
                g.AddDashboard(
                    new Compose.Config.Grafana.Dashboard()
                    {
                        DashboardData = JsonConvert.DeserializeObject(d)
                    }).Wait();

                g.Dispose();
            }
            catch (Exception)
            {
                TestContext.Out.WriteLine("Error - disposing objects.");
                g.Dispose();
                throw;
            }

            Assert.Pass();
        }
    }
}