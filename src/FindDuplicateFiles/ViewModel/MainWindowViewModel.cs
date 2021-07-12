using System.Collections.ObjectModel;
using FindDuplicateFiles.Model;

namespace FindDuplicateFiles.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SearchFolders = new ObservableCollection<string>();
            DuplicateFiles = new ObservableCollection<DuplicateFileInfo>();
        }

        private ObservableCollection<string> _searchFolders;
        /// <summary>
        /// 已选取的要搜索的文件夹
        /// </summary>
        public ObservableCollection<string> SearchFolders
        {
            get { return _searchFolders; }
            set
            {
                _searchFolders = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DuplicateFileInfo> _duplicateFiles;
        /// <summary>
        /// 重复文件集合
        /// </summary>
        public ObservableCollection<DuplicateFileInfo> DuplicateFiles
        {
            get { return _duplicateFiles; }
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
            get { return _isShowLoading; }
            set
            {
                _isShowLoading = value;
                OnPropertyChanged("IsShowLoading");
            }
        }
    }
}
