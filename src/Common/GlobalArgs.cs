using System;
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
        public static string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;
    }
}
