namespace FindDuplicateFiles.Model
{
    public class AppConfigInfo
    {
        public string Theme { get; set; }
        public string ImageExtension { get; set; }
        public string MediaExtension { get; set; }
        public string DocumentExtension { get; set; }
        /// <summary>
        /// 每次查询允许的最大重复文件数量
        /// </summary>
        public int MaxAllowDuplicateFileCount { get; set; }
    }
}
