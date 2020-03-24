using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using NLog;

namespace OmegaGraf.Compose.MetaData
{
    public static class SystemData
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static string GetMode()
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

        private static string dataPath = "";

        public static string GetRoot()
        {
            try
            {
                string dir = dataPath == "" ? Path.Join(Directory.GetCurrentDirectory(), "data")
                                            : Regex.IsMatch(dataPath, @"^((\/)|([A-Za-z]:(\\|\/)))")
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
        public static void SetRoot(string value)
        {
            dataPath = value;
        }
    }
}