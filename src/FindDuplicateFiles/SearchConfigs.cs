using System;
using System.Collections.Generic;

namespace FindDuplicateFiles
{
    public class SearchConfigs
    {
        public List<string> Folders { get; set; }
        public SearchMatchEnum SearchMatch { get; set; }
        public SearchOptionEnum SearchOption { get; set; }
    }

    /// <summary>
    /// 匹配方式
    /// </summary>
    [Flags]
    public enum SearchMatchEnum
    {
        FileName = 1,
        FileSize = 2
    }
    /// <summary>
    /// 查找选项
    /// </summary>
    [Flags]
    public enum SearchOptionEnum
    {
        IgnoreSizeZero = 1,
        IgnoreHiddenFile = 2,
        IgnoreSmallFile = 4
    }
}
