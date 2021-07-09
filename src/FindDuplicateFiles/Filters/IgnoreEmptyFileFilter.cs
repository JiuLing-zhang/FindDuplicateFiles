using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 空文件过滤器
    /// </summary>
    public class IgnoreEmptyFileFilter : IFileSearchFilter
    {
        public List<FileInfo> FilterByCondition(List<FileInfo> files)
        {
            return files.Where(x => x.Length == 0).ToList();
        }
    }
}
