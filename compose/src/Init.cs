using System;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Nancy.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using PowerArgs;

namespace OmegaGraf.Compose
{
    public class MyArgs
    {
        [ArgShortcut("-h"), ArgShortcut("--help"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgShortcut("-v"), ArgShortcut("--verbose"), ArgDescription("Enable verbose logging")]
        public bool Verbose { get; set; }

        [ArgShortcut("-l"), ArgShortcut("--test-logs"), ArgDescription("Test logging")]
        public bool Log { get; set; }

        [ArgShortcut("-p"), ArgShortcut("--path"), ArgDescription("Absolute path to store container data. Defaults to current directory."), ArgPosition(1), ArgDefaultValue("")]
        public string Path { get; set; }

        [ArgShortcut("--host"), ArgDescription("The listen address for this application."), ArgPosition(2), ArgDefaultValue("https://0.0.0.0:5001")]
        public string[] Host { get; set; }

        [ArgShortcut("-r"), ArgShortcut("--reset"), ArgDescription("Removes existing OmegaGraf containers.")]
        public bool Reset { get; set; }
    }
}