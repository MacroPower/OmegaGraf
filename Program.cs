using System;
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
                    await x.PullImage("telegraf", "latest");

                    // TODO: Pass ports, binds, and other settings
                    var id = await x.CreateContainer("telegraf");

                    await x.StartContainer(id);

                    // config file is mounted at /docker/telegraf

                    // Ideas on modifying container configs:
                    // 1. Run app on the host and easily modify as needed.
                    // 2. Run app in a container, mount a folder root to all other mounts.
                    // 3. Attach to the containers and make writes there.

                    return id;
                }
            );

            Console.WriteLine(t.Result);
        }
    }
}