using System.Collections.ObjectModel;

namespace FindDuplicateFiles.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SearchFolders = new ObservableCollection<string>();
            DuplicateFiles = new ObservableCollection<DuplicateFileModel>();
        }

        private ObservableCollection<string> _searchFolders;
        /// <summary>
        /// 已选取的要搜索的文件夹
        /// </summary>
        public ObservableCollection<string> SearchFolders
        {
            get => _searchFolders;
            set
            {
                _searchFolders = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DuplicateFileModel> _duplicateFiles;
        /// <summary>
        /// 重复文件集合
        /// </summary>
        public ObservableCollection<DuplicateFileModel> DuplicateFiles
        {
            get => _duplicateFiles;
            set
            {
                _duplicateFiles = value;
                OnPropertyChanged();
            }
        }

        private bool _isShowLoading;
        /// <summary>
        /// 是否显示loading窗口
        /// </summary>
        public bool IsShowLoading
        {
            get => _isShowLoading;
            set
            {
                _isShowLoading = value;
                OnPropertyChanged();
            }
        }

        private string _jobMessage;
        /// <summary>
        /// 任务消息
        /// </summary>
        public string JobMessage
        {
            get => _jobMessage;
            set
            {
                _jobMessage = value;
                OnPropertyChanged();
            }
        }
    }
}
