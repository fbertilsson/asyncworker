using System.Windows.Controls;
using log4net;

namespace CruncherModule.PrimeCruncher
{
    public partial class PrimeCruncherView : UserControl, IPrimeCruncherView
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PrimeCruncherView));

        public PrimeCruncherView()
        {
            Log.Debug("Enter");
            InitializeComponent();
            Log.Debug("Exit");
        }

        #region IPrimeCruncherView Members

        public PrimeCruncherPresentationModel Model
        {
            get
            {
                return (PrimeCruncherPresentationModel)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

        #endregion

        private void CheckBox_Checked_1(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
