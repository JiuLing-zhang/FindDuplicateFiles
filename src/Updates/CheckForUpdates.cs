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
            _myHttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.164 Safari/537.36 Edg/91.0.864.71");
            string json = _myHttpClient.GetStringAsync("https://api.github.com/repos/JiuLing-zhang/FindDuplicateFiles/releases").Result;
            var releases = System.Text.Json.JsonSerializer.Deserialize<List<GitHubReleases>>(json);
            if (releases == null)
            {
                throw new Exception("连接服务器失败");
            }
            var newVersion = releases.FirstOrDefault(x => x.prerelease == false);
            if (newVersion == null)
            {
                return (false, "", "");
            }

            if (newVersion.tag_name == GlobalArgs.AppVersion)
            {
                return (false, "", "");
            }

            return (true, newVersion.tag_name, newVersion.html_url);
        }
    }
}
