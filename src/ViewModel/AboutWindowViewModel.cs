namespace FindDuplicateFiles.ViewModel
{
    public class AboutWindowViewModel : ViewModelBase
    {
        private string _downloadUrl;
        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUrl
        {
            get => _downloadUrl;
            set
            {
                _downloadUrl = value;
                OnPropertyChanged();
            }
        }

        private string _version;
        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged();
            }
        }

    }
}
