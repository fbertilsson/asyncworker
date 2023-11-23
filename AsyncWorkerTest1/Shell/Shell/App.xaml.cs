using System.Windows;

namespace AsyncWorkerTest1.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Bootstrapper bootStrapper = new Bootstrapper();
            bootStrapper.Run();
        }
    }
}
