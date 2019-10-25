using System.Collections.Generic;

namespace OmegaGraf.Compose.Config.Telegraf
{
    public class Telegraf
    {
        public Inputs Inputs { get; set; }
    }

    public class Inputs
    {
        public IEnumerable<VSphere> VSphere { get; set; }
    }

    public class VSphere
    {
        public List<string> VCenters { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}