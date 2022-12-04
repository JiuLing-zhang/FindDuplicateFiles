using System;
using System.Collections.Generic;

namespace FindDuplicateFiles.Model
{
    public class SearchConfigs
    {
        public List<string> Folders { get; set; }
        public SearchMatchEnum SearchMatch { get; set; }
        public SearchOptionEnum SearchOption { get; set; }
        public SearchOptionData SearchOptionData { get; set; }
    }

    /// <summary>
    /// 匹配方式
    /// </summary>
    [Flags]
    public enum SearchMatchEnum
    {
        Name = 1,
        Size = 2,
        LastWriteTime = 4,
        MD5 = 8,
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
        IgnoreSystemFile = 64,
        OnlyFileName = 128
    }

    /// <summary>
    /// 查找选项的数据
    /// </summary>
    public class SearchOptionData
    {
        public List<string> OnlyFileNames { get; set; }
    }
}
