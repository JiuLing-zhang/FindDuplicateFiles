namespace FindDuplicateFiles.ViewModel
{
    public class SearchDirectoryModel : ViewModelBase
    {
        private string _directoryName;
        /// <summary>
        /// 已选取的要搜索的文件夹
        /// </summary>
        public string DirectoryName
        {
            get => _directoryName;
            set
            {
                _directoryName = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
