using NLog;

namespace OmegaGraf.Compose
{
    public static class Log
    {
        public static void SetLogLevel(LogLevel newLogLevel)
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                foreach (var target in rule.Targets)
                {
                    if (target.Name == "console")
                    {
                        rule.SetLoggingLevels(newLogLevel, LogLevel.Fatal);
                    }

                    if (target.Name == "file")
                    {
                        rule.SetLoggingLevels(LogLevel.Trace, LogLevel.Fatal);
                    }
                }
            }

            LogManager.ReconfigExistingLoggers();
        }
    }
}
