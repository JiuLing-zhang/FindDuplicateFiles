using System;
using System.IO;
using FindDuplicateFiles.Model;

namespace FindDuplicateFiles.Common
{
    public class GlobalArgs
    {
        /// <summary>
        /// App名称
        /// </summary>
        public static string AppName { get; set; } = AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// App Data文件夹路径
        /// </summary>
        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public static string AppConfigPath { get; set; } = Path.Combine(AppDataPath, AppName, "config.json");

        public static AppConfigInfo AppConfig { get; set; }
        /// <summary>
        /// 自动更新配置文件路径
        /// </summary>
        public const string UpdateConfigPath = "/Updates/Update";

        public static UpdateConfigInfo UpdateConfig { get; set; } = new();

        public static string AppVersion { get; set; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();

    }
}
