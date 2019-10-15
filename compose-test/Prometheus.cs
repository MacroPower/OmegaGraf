using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace OmegaGraf.Compose.Tests
{
    public class Prometheus
    {
        [Test]
        public void Deploy()
        {
            var jsonString = @"
                {
                    ""global"": {
                        ""scrapeInterval"": ""15s""
                    },
                    ""scrapeConfigs"": [
                        {
                            ""jobName"": ""prometheus"",
                            ""scrapeInterval"": ""5s"",
                            ""staticConfigs"": [{
                                ""targets"": [""localhost:9090""]
                            }]
                        }
                    ]
                }
            ";

            var dynamic = Newtonsoft.Json.JsonConvert
                          .DeserializeObject<Config.Prometheus>(jsonString);

            var x = new MetaData.Runner();

            var uuid =
                x.AddYamlConfig<Config.Prometheus>(
                    dynamic, @"C:/docker/prometheus/config/prometheus.yml"
                )
                    .Build(
                        new MetaData.BuildConfiguration()
                        {
                            Image = "prom/prometheus",
                            Tag = "latest",
                            Ports = new List<int>(){
                                9090
                            },
                            Binds = new Dictionary<string, string>()
                            {
                                { "C:/docker/prometheus/config", "/etc/prometheus" },
                                { "C:/docker/prometheus/data",   "/prometheus"     }
                            },
                            Parameters = new List<string>()
                            {
                                "--config.file=/etc/prometheus/prometheus.yml",
                                "--storage.tsdb.path=/prometheus"
                            }
                        }
                    );

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}