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
        private readonly SdkWrapper _sdkWrapper;

        public bool ToolbarVisible { get; set; } = false;

        public string Drivers { get; set; } = "mattisdasdasdSDASDASDASDASDASDASDASDASASDASASDASDSADASDASD";

        public ShellViewModel()
        {
            var wrapper = new SdkWrapper();
            _sdkWrapper = wrapper;

            _sdkWrapper.TelemetryUpdated += OnTelemetryUpdated;
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void LoadOptionsPage()
        {
            ActivateItem(new HomeViewModel());
        }
    }
}
