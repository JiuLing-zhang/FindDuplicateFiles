using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicateFiles.Model
{
    public class UpdateAppInfo
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public int versionCode { get; set; }
        public string versionName { get; set; }
        public string minVersionName { get; set; }
        public string downloadUrl { get; set; }
    }

}
