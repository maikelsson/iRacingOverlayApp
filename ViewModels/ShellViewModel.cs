using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using iRacingLiveDataOverlay.Helpers;
using iRacingSdkWrapper;

namespace iRacingLiveDataOverlay.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        IWindowManager manager = new WindowManager();
        LiveDataViewModel liveDataWindow;

        public bool ToolbarVisible { get; set; } = false;

        public string Drivers { get; set; } = "mattisdasdasdSDASDASDASDASDASDASDASDASASDASASDASDSADASDASD";

        public ShellViewModel()
        {
            LoadOptionsView();
            //liveDataWindow = new LiveDataViewModel();
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
            manager.ShowWindow(new LiveDataViewModel(), null, null);
        }

        public void CloseLiveDataWindow()
        {
            liveDataWindow.TryClose();
        }
    }
}
