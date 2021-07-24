using FindDuplicateFiles.Updates;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using FindDuplicateFiles.Common;
using MessageBox = System.Windows.MessageBox;

namespace FindDuplicateFiles
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            TxtVersion.Text = $"版本：{GlobalArgs.AppVersion}";
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

        private void BtnCheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var (isNewVersion, version, link) = new CheckForUpdates().Check();
                if (isNewVersion == false)
                {
                    MessageBox.Show($"当前版本已经是最新版本！", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.None);
                    return;
                }

                var notify = new NotifyIcon
                {
                    BalloonTipText = $"发现新版本：{version}{System.Environment.NewLine}点击更新",
                    Icon = new Icon($"{GlobalArgs.AppPath}Images\\icon.ico"),
                    Tag = link,
                    Visible = true
                };
                notify.BalloonTipClicked += notifyIcon_BalloonTipClicked;
                notify.ShowBalloonTip(5000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"检查更新失败，{ex.Message}", "重复文件查找", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            var notify = sender as NotifyIcon;
            if (notify == null)
            {
                return;
            }
            OpenUrl(notify.Tag.ToString());
        }

        private void OpenUrl(string url)
        {
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = url;
                compiler.StartInfo.UseShellExecute = true;
                compiler.Start();
            }
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
