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
        IWindowManager manager = new WindowManager();
        LiveDataViewModel liveDataWindow;
        MockLiveDataViewModel mockLiveDataWindow;

        public bool ToolbarVisible { get; set; } = false;

        public ShellViewModel()
        {
            LoadOptionsView();
        }

        public void LoadOptionsView()
        {
            ActivateItem(new OptionsViewModel());
        }

        public void LoadHomeView()
        {
            ActivateItem(new HomeViewModel(this));
        }

        public void OpenLiveDataWindow()
        {
            liveDataWindow = new LiveDataViewModel();
            manager.ShowWindow(liveDataWindow, null, null);
        }

        public void OpenMockLiveDataWindow()
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
    }
}
