using NUnit.Framework;
using OmegaGraf.Compose.MetaData;
using System;

namespace OmegaGraf.Compose.Tests
{
    public class Telegraf
    {
        [Test]
        public void Deploy()
        {
            var runner = new Runner();

            var config = new Config<Config.Telegraf.Telegraf>()
            {
                Path = @"C:\docker\telegraf\telegraf.conf",
                Data = Example.Telegraf.Config[0].Data
            };

            var uuid = runner.AddTomlConfig(x => x.LowerCase, config).Build(Example.Telegraf.BuildConfiguration);

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);

            Assert.Pass();
        }
    }
}