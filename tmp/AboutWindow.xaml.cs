using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

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

            TxtVersion.Text = $"版本：{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void GoWebsite_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = e.Uri.AbsoluteUri;
                compiler.StartInfo.UseShellExecute = true;
                compiler.Start();
            }
            e.Handled = true;
        }
         
    }
}
