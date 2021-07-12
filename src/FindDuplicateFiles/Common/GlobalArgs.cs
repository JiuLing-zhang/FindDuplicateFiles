using FindDuplicateFiles.Model;

namespace FindDuplicateFiles.Common
{
    public class GlobalArgs
    {
        public const string AppConfigPath = "/config.json";
        public static AppConfigInfo AppConfig = new();
    }
}
