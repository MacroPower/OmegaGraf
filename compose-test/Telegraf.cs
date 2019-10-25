using NUnit.Framework;
using OmegaGraf.Compose.Config.Telegraf;
using OmegaGraf.Compose.MetaData;
using System;
using System.Collections.Generic;

namespace OmegaGraf.Compose.Tests
{
    public class Telegraf
    {
        [Test]
        public void Deploy()
        {
            var conf = new Config.Telegraf.Telegraf()
            {
                Inputs = new Inputs()
                {
                    VSphere = new List<VSphere>()
                    {
                        new VSphere()
                        {
                            VCenters = new List<string>(){ "localhost:10000" },
                            Username = "test",
                            Password = "password"
                        }
                    }
                }
            };

            var config = new Config<Config.Telegraf.Telegraf>()
            {
                Path = @"C:\docker\telegraf\test.conf",
                Data = conf
            };

            var runner = new Runner();

            var uuid = runner.AddTomlConfig(x => x.LowerCase, config).Build(Example.Telegraf.BuildConfiguration);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}