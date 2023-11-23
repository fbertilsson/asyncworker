namespace AsyncWorkerTest1.Shell
{
    public class ShellPresenter
    {
        public ShellPresenter(IShellView view)
        {
            View = view;
        }

        public IShellView View { get; private set; }
    }
}