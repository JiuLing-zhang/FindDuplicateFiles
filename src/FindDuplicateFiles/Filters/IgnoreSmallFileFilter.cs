using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 小文件过滤器
    /// </summary>
    public class IgnoreSmallFileFilter : IFilePathFilter
    {
        private readonly decimal _minLength;
        /// <summary>
        /// 初始化过滤器
        /// </summary>
        /// <param name="minLength">最小的文件大小（KB）</param>
        public IgnoreSmallFileFilter(decimal minLength)
        {
            _minLength = minLength;
        }
        public List<FileInfo> FilterByCondition(List<FileInfo> files)
        {
            return files.Where(x => x.Length / 1024m < _minLength).ToList();
        }
    }
}
