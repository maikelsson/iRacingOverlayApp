using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;
using iRacingLiveDataOverlay.Services;

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Screen
    {

        private readonly SdkWrapper _wrapper;

        public LiveDataViewModel()
        {
            _wrapper = IRacingService._wrapper;
            _wrapper.SessionInfoUpdated += new EventHandler<SdkWrapper.SessionInfoUpdatedEventArgs>(_wrapper_SessionInfoUpdated);
        }

        private void _wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            SessionInfo = e.SessionInfo.TryGetValue("").ToString();
        }

        private IObservableCollection<TelemetryInfo> _telemetryInfos;
        public IObservableCollection<TelemetryInfo> TelemetryInfos
        {
            get
            {
                return _telemetryInfos;
            }
            set
            {
                _telemetryInfos = value;
                NotifyOfPropertyChange(() => TelemetryInfos);
            }
        }

        private string _sessionInfo;
        public string SessionInfo
        {
            get
            {
                return _sessionInfo;
            }
            set
            {
                _sessionInfo = value;
                NotifyOfPropertyChange(() => SessionInfo);
            }
        }


    }
}
