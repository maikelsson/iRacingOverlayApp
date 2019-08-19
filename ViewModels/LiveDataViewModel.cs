using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;
using iRacingLiveDataOverlay.Services;
using iRacingSimulator.Drivers;
using iRacingSimulator;
using System.Collections.ObjectModel;

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Screen
    {

        private readonly SdkWrapper _wrapper;

        public LiveDataViewModel()
        {
            _wrapper = IRacingService._wrapper;
            _wrapper.TelemetryUpdateFrequency = 10;
            //_wrapper.SessionInfoUpdated += new EventHandler<SdkWrapper.SessionInfoUpdatedEventArgs>(_wrapper_SessionInfoUpdated);
            _wrapper.TelemetryUpdated += new EventHandler<SdkWrapper.TelemetryUpdatedEventArgs>(_wrapper_TelemetryUpdated);
            _currentDrivers = new ObservableCollection<Driver>();
            _wrapper.RequestSessionInfoUpdate();
        }

        private void _wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            //_drivers.Clear();

            foreach(var driver in Sim.Instance.Drivers)
            {
                _currentDrivers.Add(driver);
                DriverInfo = $"{driver.Name.ToString()}, {driver.License.ToString()}";
            }

            SessionInfo = $"{e.TelemetryInfo.TrackTemp.Value.ToString()} C";
            //SessionInfo = e.TelemetryInfo.SessionTimeRemain.Value.ToString();

        }

        //private void _wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        //{
        //    SessionInfo = _wrapper.GetTelemetryValue<float>("TrackTemp").ToString();
        //}

        private string _driverInfo;
        public string DriverInfo
        {
            get
            {
                return _driverInfo;
            }
            set
            {
                _driverInfo = value;
                NotifyOfPropertyChange(() => DriverInfo);
            }
        }

        private ObservableCollection<Driver> _currentDrivers;
        public ObservableCollection<Driver> CurrentDrivers
        {
            get
            {
                return _currentDrivers;
            }
            set
            {
                _currentDrivers = value;
                NotifyOfPropertyChange(() => CurrentDrivers);
            }
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
