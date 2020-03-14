using System;
using System.IO;
using System.Runtime.InteropServices;
using NLog;

namespace OmegaGraf.Compose.MetaData
{
    public static class SystemData
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static string Mode
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return ":rw";
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "";
                }

                var e = new NotSupportedException();

                logger.Error(e, "OS Platform is not supported");
                throw e;
            }
        }

        private static string dataPath = "";
        public static string Root
        {
            get
            {
                try
                {
                    string dir = dataPath == "" ? Path.Join(Directory.GetCurrentDirectory(), "data")
                                                : dataPath.StartsWith("/") || dataPath.StartsWith(@"\")
                                                    ? dataPath
                                                    : Path.Join(Directory.GetCurrentDirectory(), dataPath);

                    return dir;
                }
                catch (Exception e)
                {
                    logger.Error(e, "Could not determine the current execution path");
                    throw;
                }
            }
            set
            {
                dataPath = value;
            }
        }
    }
}