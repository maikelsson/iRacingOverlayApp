using iRacingSimulator;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
