using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using FindDuplicateFiles.Extensions;

namespace FindDuplicateFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 已选取的要搜索的文件夹
        /// </summary>
        public ObservableCollection<string> SearchFolders { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            InitializeSearchConfig();
            InitializeLoading();

            SearchFolders = new ObservableCollection<string>();
            this.DataContext = this;
        }

        private void InitializeSearchConfig()
        {
            //匹配方式
            ChkFileName.IsChecked = true;
            ChkFileSize.IsChecked = false;

            //选项
            ChkSize0.IsChecked = true;
            ChkHiddenFile.IsChecked = true;
            ChkSmallFile.IsChecked = true;
        }
        private void InitializeLoading()
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
            PanelLoading.Height = 0;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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

            if (SearchFolders.Contains(path))
            {
                return;
            }

            SearchFolders.Add(path);
            //  this.DataContext = this;
        }

        private void BtnRemoveSearchFolder_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button)?.Tag;
            if (tag == null)
            {
                return;
            }

            SearchFolders.Remove(tag.ToString());
        }

        public string JobFile { get; set; }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (SearchFolders.Count == 0)
            {
                MessageBox.Show("请选择要查找的文件夹");
                return;
            }
            PanelLoading.Height = 25;
            Task.Run(() =>
                  {
                      var t = System.IO.Directory.GetFiles(@"D:\Work\Doc\1Other");
                      foreach (var s in t)
                      {
                          JobFile = s;
                          System.Threading.Thread.Sleep(500);
                      }
                  });
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
