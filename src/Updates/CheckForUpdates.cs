using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FindDuplicateFiles.Common;
using FindDuplicateFiles.Model;

namespace FindDuplicateFiles.Updates
{
    public class CheckForUpdates
    {
        private static HttpClient _myHttpClient;
        public CheckForUpdates()
        {
            _myHttpClient = new HttpClient();
        }
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        public (bool isNewVersion, string version, string link) Check()
        {
            string json = _myHttpClient.GetStringAsync(GlobalArgs.UpdateConfig.Url).Result;
            var appInfo = System.Text.Json.JsonSerializer.Deserialize<UpdateAppInfo>(json);
            if (appInfo == null)
            {
                throw new Exception("连接服务器失败");
            }

            if (appInfo.code != 0)
            {
                throw new Exception(appInfo.message);
            }
            Version newVersion = new Version(appInfo.data.versionName);
            Version currentVersion = new Version(GlobalArgs.AppVersion);

            var result = JiuLing.CommonLibs.VersionUtils.CheckNeedUpdate(newVersion, currentVersion);

            if (result == false)
            {
                return (false, "", "");
            }
            return (true, appInfo.data.versionName, appInfo.data.downloadUrl);
        }
    }
}
