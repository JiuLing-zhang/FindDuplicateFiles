using System;

namespace FindDuplicateFiles.ViewModel
{
    public class DuplicateFileModel : ViewModelBase
    {
        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged();
            }
        }

        private bool _isCheckedFile;
        public bool IsCheckedFile
        {
            get => _isCheckedFile;
            set
            {
                _isCheckedFile = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        private decimal _size;
        public decimal Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastWriteTime;
        public DateTime LastWriteTime
        {
            get => _lastWriteTime;
            set
            {
                _lastWriteTime = value;
                OnPropertyChanged();
            }
        }


        private string _extension;
        public string Extension
        {
            get => _extension;
            set
            {
                _extension = value;
                OnPropertyChanged();
            }
        }
    }
}
