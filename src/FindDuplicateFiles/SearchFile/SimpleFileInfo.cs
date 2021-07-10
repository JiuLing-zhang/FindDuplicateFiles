using System;

namespace FindDuplicateFiles.SearchFile
{
    public class SimpleFileInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public decimal Size { get; set; }
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        /// 文件后缀名
        /// </summary>
        public string Extension { get; set; }
    }
}
