using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using FindDuplicateFiles.Common;
using FindDuplicateFiles.ViewModel;
using System.Threading.Tasks;
using FindDuplicateFiles.Updates;

namespace FindDuplicateFiles
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        private readonly AboutWindowViewModel _myModel = new();
        public AboutWindow()
        {
            InitializeComponent();
            DataContext = _myModel;
            _myModel.Version = $"版本：{GlobalArgs.AppVersion}";
            _myModel.DownloadUrl = "https://github.com/JiuLing-zhang";
        }
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void GoWebsite_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            OpenUrl(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void BtnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            _myModel.UpdateMessage = "正在查找更新....";
            Task.Run(() =>
            {
                try
                {
                    var (isNewVersion, version, link) = new CheckForUpdates().Check();
                    if (isNewVersion == false)
                    {
                        _myModel.UpdateMessage = "当前版本已经是最新版本！";
                        return;
                    }
                    _myModel.UpdateMessage = $"发现新版本：{version}";
                    _myModel.DownloadUrl = link;
                }
                catch (Exception ex)
                {
                    _myModel.UpdateMessage = $"检查更新失败，{ex.Message}";
                }
            });
        }
        private void OpenUrl(string url)
        {
            using Process compiler = new Process();
            compiler.StartInfo.FileName = url;
            compiler.StartInfo.UseShellExecute = true;
            compiler.Start();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
