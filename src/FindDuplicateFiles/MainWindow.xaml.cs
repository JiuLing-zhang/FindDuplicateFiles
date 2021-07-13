using FindDuplicateFiles.Common;
using FindDuplicateFiles.Extensions;
using FindDuplicateFiles.SearchFile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using FindDuplicateFiles.Model;
using FindDuplicateFiles.ViewModel;

namespace FindDuplicateFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 是否正在进行搜索
        /// </summary>
        private bool _isSearching;

        private readonly SearchFilesJob _searchFilesJob = new();
        private readonly MainWindowViewModel _myModel = new();
        public MainWindow()
        {
            InitializeComponent();

            LoadingAppConfig();
            LoadingTheme(GlobalArgs.AppConfig.Theme);

            BindingItemsSource();
            BindingSearchEvent();

            InitializeSearchCondition();
            InitializeLoadingBlock();
        }

        private void LoadingAppConfig()
        {
            string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}{GlobalArgs.AppConfigPath}";
            string configString = File.ReadAllText(configPath);
            GlobalArgs.AppConfig = System.Text.Json.JsonSerializer.Deserialize<AppConfigInfo>(configString);
        }

        private void InitializeSearchCondition()
        {
            //匹配方式
            ChkFileName.IsChecked = true;
            ChkFileSize.IsChecked = true;
            ChkFileLastWriteTimeUtc.IsChecked = true;

            //选项
            ChkIgnoreEmptyFile.IsChecked = true;
            ChkIgnoreHiddenFile.IsChecked = true;
            ChkIgnoreSmallFile.IsChecked = false;
            RdoAllFile.IsChecked = true;

            _myModel.SearchFolders.Clear();
        }


        /// <summary>
        /// UI数据绑定
        /// </summary>
        private void BindingItemsSource()
        {
            ListBoxSearchFolders.ItemsSource = _myModel.SearchFolders;
            ListViewDuplicateFile.ItemsSource = _myModel.DuplicateFiles;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewDuplicateFile.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Key");
            view.GroupDescriptions?.Add(groupDescription);

            RdoOnlyImageFile.ToolTip = $"仅支持：{GlobalArgs.AppConfig.ImageExtension}文件";
            RdoOnlyMediaFile.ToolTip = $"仅支持：{GlobalArgs.AppConfig.MediaExtension}文件";
            RdoOnlyDocumentFile.ToolTip = $"仅支持：{GlobalArgs.AppConfig.DocumentExtension}文件";
            DataContext = _myModel;
        }

        /// <summary>
        /// 绑定查找任务的内部事件
        /// </summary>
        private void BindingSearchEvent()
        {
            _searchFilesJob.EventMessage = ExecutedMessage;
            _searchFilesJob.EventDuplicateFound = DuplicateFilesFound;
            _searchFilesJob.EventSearchFinished = SearchFinished;
        }
        private void InitializeLoadingBlock()
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                RepeatBehavior = RepeatBehavior.Forever
            };
            RotateTransform rt = new RotateTransform();
            ImgLoading.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        private void ExecutedMessage(string message)
        {
            _myModel.JobMessage = message;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

            if (GridImage.Visibility == Visibility)
            {
                ImgSelected.Source = null;
                GridImage.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.ImgMaximize.Source = new BitmapImage(new Uri("pack://application:,,,/Images/maximize.png"));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.ImgMaximize.Source = new BitmapImage(new Uri("pack://application:,,,/Images/restore.png"));
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAddSearchFolder_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            var path = fbd.SelectedPath;
            if (path.IsEmpty())
            {
                return;
            }

            if (_myModel.SearchFolders.Contains(path))
            {
                return;
            }
            _myModel.SearchFolders.Add(path);
        }

        private void BtnRemoveSearchFolder_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button)?.Tag;
            if (tag == null)
            {
                return;
            }

            _myModel.SearchFolders.Remove(tag.ToString());
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (_isSearching)
            {
                if (MessageBox.Show("确定要停止搜索吗？", "重复文件查找", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                {
                    return;
                }
                EndSearch();
            }
            else
            {
                BeginSearch();
            }
        }

        private void BeginSearch()
        {
            if (_myModel.SearchFolders.Count == 0)
            {
                MessageBox.Show("请选择要查找的文件夹", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            //匹配方式校验
            SearchMatchEnum searchMatch = 0;
            if (ChkFileName.IsChecked == true)
            {
                searchMatch = searchMatch | SearchMatchEnum.Name;
            }
            if (ChkFileSize.IsChecked == true)
            {
                searchMatch = searchMatch | SearchMatchEnum.Size;
            }
            if (ChkFileLastWriteTimeUtc.IsChecked == true)
            {
                searchMatch = searchMatch | SearchMatchEnum.LastWriteTime;
            }
            if (searchMatch == 0)
            {
                MessageBox.Show("请选择匹配方式", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            //选项
            SearchOptionEnum searchOption = 0;
            if (ChkIgnoreEmptyFile.IsChecked == true)
            {
                searchOption = searchOption | SearchOptionEnum.IgnoreEmptyFile;
            }
            if (ChkIgnoreHiddenFile.IsChecked == true)
            {
                searchOption = searchOption | SearchOptionEnum.IgnoreHiddenFile;
            }
            if (ChkIgnoreSmallFile.IsChecked == true)
            {
                searchOption = searchOption | SearchOptionEnum.IgnoreSmallFile;
            }
            if (RdoOnlyImageFile.IsChecked == true)
            {
                searchOption = searchOption | SearchOptionEnum.OnlyImageFile;
            }
            if (RdoOnlyMediaFile.IsChecked == true)
            {
                searchOption = searchOption | SearchOptionEnum.OnlyMediaFile;
            }
            if (RdoOnlyDocumentFile.IsChecked == true)
            {
                searchOption = searchOption | SearchOptionEnum.OnlyDocumentFile;
            }

            SetBeginSearchStyle();
            _myModel.DuplicateFiles.Clear();
            var config = new SearchConfigs()
            {
                Folders = new List<string>(_myModel.SearchFolders.ToList()),
                SearchMatch = searchMatch,
                SearchOption = searchOption
            };
            _searchFilesJob.Start(config);
        }

        /// <summary>
        /// 开始搜索
        /// </summary>
        private void SetBeginSearchStyle()
        {
            TxtSearch.Text = "停止";
            ImgSearch.Source = new BitmapImage(new Uri("pack://application:,,,/Images/stop.png"));
            _isSearching = true;
            _myModel.IsShowLoading = _isSearching;
        }

        private void EndSearch()
        {
            SetEndSearchStyle();
            _searchFilesJob.Stop();
        }

        /// <summary>
        /// 结束搜索
        /// </summary>
        private void SetEndSearchStyle()
        {
            TxtSearch.Text = "开始";
            ImgSearch.Source = new BitmapImage(new Uri("pack://application:,,,/Images/search.png"));
            _isSearching = false;
            _myModel.IsShowLoading = _isSearching;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (_isSearching)
            {
                MessageBox.Show("任务执行中，禁止重置", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            InitializeSearchCondition();
        }

        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {

            var tag = (sender as Button)?.Tag;
            if (tag == null)
            {
                MessageBox.Show("修改主题失败：系统错误", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            string theme = tag.ToString();

            LoadingTheme(theme);
            GlobalArgs.AppConfig.Theme = theme;
            string appConfigString = System.Text.Json.JsonSerializer.Serialize(GlobalArgs.AppConfig);
            string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}{GlobalArgs.AppConfigPath}";
            File.WriteAllText(configPath, appConfigString);
        }

        private void LoadingTheme(string themeName)
        {
            try
            {
                var mResourceSkin = new ResourceDictionary()
                {
                    Source = new Uri($"/Themes/Theme{themeName}.xaml", UriKind.RelativeOrAbsolute)
                };
                Application.Current.Resources.MergedDictionaries[0] = mResourceSkin;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"主题加载失败，{ex.Message}", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DuplicateFilesFound(string key, SimpleFileInfo simpleFile)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _myModel.DuplicateFiles.Add(new DuplicateFileModel()
                {
                    Key = key,
                    Name = simpleFile.Name,
                    Path = simpleFile.Path,
                    Size = Math.Ceiling(simpleFile.Size / 1024),
                    LastWriteTime = simpleFile.LastWriteTime,
                    Extension = simpleFile.Extension,
                    IsCheckedFile = false
                });
            });
        }
        private void SearchFinished()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SetEndSearchStyle();
                MessageBox.Show("查找完成", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private void ListViewDuplicateFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            if (!(lv?.SelectedItem is DuplicateFileModel selectFile))
            {
                return;
            }
            if (selectFile.Extension.IsNotEmpty() && GlobalArgs.AppConfig.ImageExtension.Contains(selectFile.Extension))
            {
                ImgSelected.Source = new BitmapImage(new Uri(selectFile.Path, UriKind.Absolute));
                GridImage.Visibility = Visibility.Visible;
            }
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow
            {
                Owner = this
            };
            about.Show();
        }

        private void BtnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            if (_myModel.DuplicateFiles.Count == 0)
            {
                MessageBox.Show("没有可用数据", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (_myModel.DuplicateFiles.Any(x => x.IsCheckedFile))
            {
                RemoveChooseAllFiles();
            }
            else
            {
                ChooseAllFiles();
            }

        }
        private void ChooseAllFiles()
        {
            foreach (var duplicateFile in _myModel.DuplicateFiles)
            {
                duplicateFile.IsCheckedFile = true;
            }
            var keyList = _myModel.DuplicateFiles.GroupBy(x => x.Key);
            foreach (var keyItem in keyList)
            {
                _myModel.DuplicateFiles.First(x => x.Key == keyItem.Key).IsCheckedFile = false;
            }
        }
        private void RemoveChooseAllFiles()
        {
            foreach (var duplicateFile in _myModel.DuplicateFiles)
            {
                duplicateFile.IsCheckedFile = false;
            }
        }
        private void BtnDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_myModel.DuplicateFiles.Count == 0)
                {
                    MessageBox.Show("没有可用数据", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show($"确认要删除选中文件吗？文件删除后不可恢复！", "重复文件查找", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }

                foreach (var file in _myModel.DuplicateFiles)
                {
                    if (file.IsCheckedFile == false)
                    {
                        continue;
                    }
                    System.IO.File.Delete(file.Path);
                }
                MessageBox.Show("删除完成", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除失败：{ex.Message}", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
