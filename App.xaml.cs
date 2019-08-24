using iRacingSimulator;
using System.Windows;

namespace iRacingLiveDataOverlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Sim.Instance.Stop();
        }
    }
}
