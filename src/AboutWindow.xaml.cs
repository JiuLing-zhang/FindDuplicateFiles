using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using FindDuplicateFiles.Common;
using FindDuplicateFiles.ViewModel;

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
            var app = JiuLing.AutoUpgrade.Shell.AutoUpgradeFactory.Create();
            app.SetUpgrade(x =>
            {
                x.IsCheckSign = true;
            });
            app.UseHttpMode(Resource.AutoUpgradePath).Run();
        }
        private static void OpenUrl(string url)
        {
            using var compiler = new Process();
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
