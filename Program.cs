using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OmegaGraf.Compose
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new Docker();

            // Parallel.ForEach -
            // Create all containers simultaneously
            // Run container creation steps synchronously
            var t = Task.Run(
                async () =>
                {
                    await x.PullImage("prom/prometheus", "latest");

                    var id = await x.CreateContainer(
                        "prom/prometheus",
                        new List<int>(){ 9090 },
                        new Dictionary<string, string>()
                        {
                            { "C:/docker/prometheus/config", "/etc/prometheus" },
                            { "C:/docker/prometheus/data",   "/prometheus"     }
                        },
                        new List<string>()
                        {
                            "--config.file=/etc/prometheus/prometheus.yml",
                            "--storage.tsdb.path=/prometheus"
                        }
                    );

                    await x.StartContainer(id);

                    // config file is mounted at /docker/telegraf

                    // Ideas on modifying container configs:
                    // 1. Run app on the host and easily modify as needed.
                    // 2. Run app in a container, mount a folder root to all other mounts.
                    // 3. Attach to the containers and make writes there.

                    return id;
                }
            );

            var uuid = t.Result;

            Console.WriteLine("docker container logs " + uuid);
            Console.WriteLine("docker container inspect " + uuid);
            Console.WriteLine("docker container stop " + uuid);
            Console.WriteLine("docker container rm " + uuid);
        }
    }
}