using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingLiveDataOverlay.Helpers;
using iRacingSdkWrapper;

namespace iRacingLiveDataOverlay.ViewModels
{
    public class LiveDataViewModel : INPCBase
    {
        private readonly SdkWrapper _sdkWrapper;

        public string Drivers { get; set; } = "mattisdasdasdSDASDASDASDASDASDASDASDASASDASASDASDSADASDASD";

        public LiveDataViewModel()
        {
            var wrapper = new SdkWrapper();
            _sdkWrapper = wrapper;

            _sdkWrapper.TelemetryUpdated += OnTelemetryUpdated;
        }

        private void OnTelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
