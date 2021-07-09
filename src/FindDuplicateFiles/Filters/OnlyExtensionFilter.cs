﻿using System;
using System.Collections.Generic;
using System.IO;

namespace FindDuplicateFiles.Filters
{
    /// <summary>
    /// 指定扩展名过滤器
    /// </summary>
    public class OnlyExtensionFilter : IFileSearchFilter
    {
        private readonly List<string> _extensions;
        /// <summary>
        /// 初始化过滤器
        /// </summary>
        /// <param name="extensions">指定的扩展名列表</param>
        public OnlyExtensionFilter(List<string> extensions)
        {
            _extensions = extensions;
        }
        public List<FileInfo> FilterByCondition(List<FileInfo> files)
        {
            throw new NotImplementedException();
        }
    }
}
