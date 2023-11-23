using System.Windows;

namespace AsyncWorkerTest1.Shell
{
    /// <summary>
    /// Technology specific interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, IShellView
    {
        public Shell()
        {
            InitializeComponent();
        }

        public void ShowView()
        {
            Show();
        }
    }
}
