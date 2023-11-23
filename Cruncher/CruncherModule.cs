using CruncherModule.PrimeCruncher;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using log4net;

namespace CruncherModule
{
    /// <summary>
    /// Defines a Composite Application Library module for showing the async worker.
    /// Contains initialization logic.
    /// </summary>
    public class CruncherModule : IModule
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CruncherModule));

        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public CruncherModule(IUnityContainer container, IRegionManager regionManager)
        {
            Log.Debug("Enter");

            Container = container;
            RegionManager = regionManager;

            Log.Debug("Exit");
        }

        /// <summary>
        /// Initializes the module.
        /// Make sure the module is initialized, e.g. in the Bootstrapper.GetModuleCatalog() method 
        /// or declaratively in the shell's App.config.
        /// </summary>
        public void Initialize()
        {
            Log.Debug("Enter");

            RegisterTypes();

            // Create presentation models
            var model = Container.Resolve<PrimeCruncherPresentationModel>();

            // Add views to regions
            RegionManager.Regions["MainRegion"].Add(model.View);

            //MessageBox.Show(string.Format("Module {0} successfully loaded.", typeof(CruncherModule))); 

            Log.Info("Module successfully loaded.");
            Log.Debug("Exit");
        }



        /// <summary>
        /// Registers types, e.g. controllers, views, and services.
        /// </summary>
        private void RegisterTypes()
        {
            Log.Debug("Enter");

            Container.RegisterType<IPrimeCruncherView, PrimeCruncherView>();

            Log.Debug("Exit");
        }
    }
}
