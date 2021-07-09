using System.Collections.Generic;
using System.IO;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 文件过滤器
    /// </summary>
    interface IFilePathFilter
    {
        /// <summary>
        /// 根据条件过滤
        /// </summary>
        /// <param name="files">文件列表</param>
        /// <returns>符合条件的文件列表</returns>
        public List<FileInfo> FilterByCondition(List<FileInfo> files);
    }
}
