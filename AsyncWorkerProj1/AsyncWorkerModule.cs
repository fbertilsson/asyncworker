using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows; // TODO remove this debug line
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using log4net;

namespace AsyncWorkerProj1
{
    /// <summary>
    /// Defines a Composite Application Library module for TODO
    /// Contains initialization logic.
    /// </summary>
    public class AsyncWorkerModule : IModule
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AsyncWorkerModule));

        private IUnityContainer Container { get; set; }
        private IRegionManager RegionManager { get; set; }

        public AsyncWorkerModule(IUnityContainer container, IRegionManager regionManager)
        {
            Log.Debug("Enter");

            Container = container;
            RegionManager = regionManager;

            Log.Debug("Exit");
        }

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public void Initialize()
        {
            Log.Debug("Enter");

            RegisterTypes();

            // Create presentation models
            // TODO: e.g. HelpRegionPresentationModel helpRegionPresentationModel = m_container.Resolve<IHelpRegionPresentationModel>();

            // Add views to regions
            // TODO: e.g. RegionManager.Regions["HelpRegion"].Add(helpRegionPresentationModel.View);

            //MessageBox.Show(string.Format("Module {0} successfully loaded.", typeof(AsyncWorkerModule))); 

            Log.Info("Module successfully loaded.");
            Log.Debug("Exit");
        }



        /// <summary>
        /// Registers types, e.g. controllers, views, and services.
        /// </summary>
        private void RegisterTypes()
        {
            Log.Debug("Enter");

            // TODO: Register types, e.g. controllers, presentation models, views, and services.
            // E.g. Container.RegisterType<IHelpRegionView, HelpRegionView>();
            // E.g. Container.RegisterType<IHelpRegionPresentationModel, HelpRegionPresentationModel>();

            Log.Debug("Exit");
        }
    }
}
