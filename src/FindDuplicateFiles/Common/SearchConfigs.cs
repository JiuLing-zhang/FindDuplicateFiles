using System;
using System.Collections.Generic;

namespace FindDuplicateFiles.Common
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
        Name = 1,
        Size = 2,
        LastWriteTime = 4
    }
    /// <summary>
    /// 查找选项
    /// </summary>
    [Flags]
    public enum SearchOptionEnum
    {
        IgnoreEmptyFile = 1,
        IgnoreHiddenFile = 2,
        IgnoreSmallFile = 4,
        OnlyImageFile = 8,
        OnlyMediaFile = 16,
        OnlyDocumentFile = 32,
    }
}
