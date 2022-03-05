using System.Collections.ObjectModel;

namespace FindDuplicateFiles.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SearchDirectory = new ObservableCollection<SearchDirectoryModel>();
            DuplicateFiles = new ObservableCollection<DuplicateFileModel>();
        }

        private ObservableCollection<SearchDirectoryModel> _searchDirectory;
        /// <summary>
        /// 要搜索的文件夹集合
        /// </summary>
        public ObservableCollection<SearchDirectoryModel> SearchDirectory
        {
            get => _searchDirectory;
            set
            {
                _searchDirectory = value;
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
