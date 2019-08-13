using Caliburn.Micro;
using iRacingLiveDataOverlay.ViewModels;
using System.Windows;

namespace iRacingLiveDataOverlay
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}