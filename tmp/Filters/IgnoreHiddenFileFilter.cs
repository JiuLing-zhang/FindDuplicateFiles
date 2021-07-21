using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 隐藏文件过滤器
    /// </summary>
    public class IgnoreHiddenFileFilter : IFileSearchFilter
    {
        public List<FileInfo> FilterByCondition(List<FileInfo> files)
        {
            return files.Where(x => x.Attributes != FileAttributes.Hidden).ToList();
        }
    }
}
