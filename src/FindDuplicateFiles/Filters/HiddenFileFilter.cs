using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 隐藏文件过滤器
    /// </summary>
    public class HiddenFileFilter : IFilePathFilter
    {
        public List<FileInfo> FilterByCondition(List<FileInfo> files)
        {
            return files.Where(x => x.Attributes == FileAttributes.Hidden).ToList();
        }
    }
}
