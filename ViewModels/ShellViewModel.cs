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
        IWindowManager manager;
        LiveDataViewModel liveDataWindow;
        MockLiveDataViewModel mockLiveDataWindow;

        public bool ToolbarVisible { get; set; } = false;

        public ShellViewModel()
        {
            manager = new WindowManager();
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
                    Sim.Instance.Start();
                });
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
