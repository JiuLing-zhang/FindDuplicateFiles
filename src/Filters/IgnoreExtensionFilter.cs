using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 忽略扩展名过滤器
    /// </summary>
    public class IgnoreExtensionFilter : IFileSearchFilter
    {
        private readonly List<string> _extensions;
        public IgnoreExtensionFilter(List<string> extensions)
        {
            _extensions = extensions;
        }
        public List<FileInfo> FilterByCondition(List<FileInfo> files)
        {
            return files.Where(x => _extensions.Contains(x.Extension.ToLower()) == false).ToList();
        }
    }
}
