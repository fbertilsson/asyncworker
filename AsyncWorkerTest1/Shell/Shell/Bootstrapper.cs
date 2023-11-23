using System.Windows;
using AsyncWorkerProj1;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;

namespace AsyncWorkerTest1.Shell
{
    /// <summary>
    /// Defines a bootstrapping sequence that registers assets in a IUnityContainer, 
    /// defines which modules should be initialized (see <see cref="ConfigureModuleCatalog"/>)
    /// and provides hooks for application ending.
    /// This class is public to be able to unit test, do not call methods on this class from elsewhere!
    /// </summary>
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        protected override void InitializeShell()
        {
            App.Current.MainWindow = (Shell) Shell;
            App.Current.MainWindow.Show();
        }
        
        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog.Initialize();
            var moduleCatalog = (ModuleCatalog) ModuleCatalog;
            moduleCatalog
                .AddModule(typeof (AsyncWorkerModule))
                .AddModule(typeof (CruncherModule.CruncherModule));
        }
    }
}
