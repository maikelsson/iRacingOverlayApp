using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using iRacingLiveDataOverlay.Helpers;
using iRacingSdkWrapper;
using iRacingSimulator;

namespace iRacingLiveDataOverlay.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private IWindowManager manager;
        private LiveDataViewModel liveDataWindow;
        private MockLiveDataViewModel mockLiveDataWindow;
        private readonly Sim _sim;

        private string _simConnectionStatus;
        public string SimConnectionStatus
        {
            get
            {
                return _simConnectionStatus;
            }
            set
            {
                _simConnectionStatus = value;
                NotifyOfPropertyChange(() => SimConnectionStatus);
            }
        }

        public bool ToolbarVisible { get; set; } = false;

        public ShellViewModel()
        {
            manager = new WindowManager();
            _sim = Sim.Instance;
            _sim.Start();
            _sim.Sdk.Connected += OnSdkConnected;
            _sim.Sdk.Disconnected += OnSdkDisconnected;
            CheckSimStatus();
        }
        
        private void OnSdkDisconnected(object sender, EventArgs e)
        {
            CheckSimStatus();
        }

        private void OnSdkConnected(object sender, EventArgs e)
        {
            CheckSimStatus();
        }

        private void CheckSimStatus()
        {

            if (_sim.Sdk.IsRunning)
            {
                SimConnectionStatus = "Sdk running";
            }
            else
            {
                SimConnectionStatus = "Sdk not running";
            }
        }

        public void OpenLiveDataWindow()
        {
            liveDataWindow = new LiveDataViewModel();
            manager.ShowWindow(liveDataWindow, null, null);
        }

        public void OpenMockDataWindow()
        {
            mockLiveDataWindow = new MockLiveDataViewModel();
            manager.ShowWindow(mockLiveDataWindow, null, null);
        }

        public void CloseLiveDataWindow()
        {
            //Check if window is open
            if (liveDataWindow == null)
                return;

            liveDataWindow.TryClose();
        }

        //Trying to initialize sim instance
        private async Task InitializeSim()
        {
            try
            {
                await Task.Run(() =>
                {
                    _sim.Start();
                });
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
