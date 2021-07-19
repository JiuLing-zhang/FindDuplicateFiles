using FindDuplicateFiles.Model;

namespace FindDuplicateFiles.Common
{
    public class GlobalArgs
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public const string AppConfigPath = "/config.json";
        public static AppConfigInfo AppConfig = new();
    }
}
