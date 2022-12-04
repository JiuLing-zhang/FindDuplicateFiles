using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 指定文件名过滤器
    /// </summary>
    internal class OnlyFileNameFilter : IFileSearchFilter
    {
        private readonly List<string> _fileNames;
        /// <summary>
        /// 初始化过滤器
        /// </summary>
        /// <param name="fileNames">指定的文件名列表</param>
        public OnlyFileNameFilter(List<string> fileNames)
        {
            _fileNames = fileNames;
        }
        public List<FileInfo> FilterByCondition(List<FileInfo> files)
        {
            return files.Where(x => _fileNames.Contains(x.Name.ToLower())).ToList();
        }
    }
}
