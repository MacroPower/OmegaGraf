using PowerArgs;

namespace OmegaGraf.Compose
{
    public class MyArgs
    {
        [ArgShortcut("-h"), ArgShortcut("--help"), ArgDescription("Shows this help and exits.")]
        public bool Help { get; set; }

        [ArgShortcut("--version"), ArgDescription("Prints version info and exits.")]
        public bool Version { get; set; }

        [ArgShortcut("-v"), ArgShortcut("--verbose"), ArgDescription("Enables verbose logging.")]
        public bool Verbose { get; set; }

        [ArgShortcut("-p"), ArgShortcut("--path"), ArgDescription("Sets absolute path to container data. Defaults to working directory."), ArgDefaultValue("")]
        public string Path { get; set; }

        [ArgShortcut("--host"), ArgDescription("Sets the listen addresses for this application."), ArgDefaultValue("http://0.0.0.0:5000")]
        public string[] Host { get; set; }

        [ArgShortcut("-s"), ArgShortcut("--sock"), ArgDescription("Overrides the Docker socket path.")]
        public string Socket { get; set; }

        [ArgShortcut("-k"), ArgShortcut("--key"), ArgDescription("Overrides the OmegaGraf Secure Key.")]
        public string Key { get; set; }

        [ArgShortcut("-r"), ArgShortcut("--reset"), ArgDescription("Removes existing OmegaGraf containers.")]
        public bool Reset { get; set; }

        
    }
}