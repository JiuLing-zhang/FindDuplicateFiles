namespace FindDuplicateFiles.Model
{
    public class AppConfigInfo
    {
        public string Theme { get; set; } = "Dark";
        public string ImageExtension { get; set; } = ".gif;.jpg;.jpeg;.png;.bmp";
        public string MediaExtension { get; set; } = ".mp3;.wma;.wav;.mpeg;.avi;.mov;.wmv;.rm;.rmvb;.flv";
        public string SystemExtension { get; set; } = ".dll";
        public string DocumentExtension { get; set; } = ".txt;.pdf;.doc;.docx;.xls;.xlsx;.ppt;.pptx";

        /// <summary>
        /// 每次查询允许的最大重复文件数量
        /// </summary>
        public int MaxAllowDuplicateFileCount { get; set; } = 500;
    }
}
