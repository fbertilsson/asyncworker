using System.Windows.Threading;

namespace CruncherModule.PrimeCruncher
{
    public interface IPrimeCruncherView
    {
        Dispatcher Dispatcher { get; }
        PrimeCruncherPresentationModel Model { get; set; }
    }
}
