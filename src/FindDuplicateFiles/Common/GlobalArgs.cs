using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicateFiles.Common
{
    public class GlobalArgs
    {
        public const string AppConfigPath = "/config.json";
        public static AppConfigInfo AppConfig = new();
    }
}
