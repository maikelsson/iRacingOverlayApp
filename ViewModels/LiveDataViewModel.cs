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
        private List<DriverModel> _drivers = new List<DriverModel>();

        private readonly SdkWrapper _wrapper;

        public LiveDataViewModel()
        {
            _wrapper = IRacingService._wrapper;
            _wrapper.TelemetryUpdateFrequency = 10;
            _wrapper.SessionInfoUpdated += new EventHandler<SdkWrapper.SessionInfoUpdatedEventArgs>(_wrapper_SessionInfoUpdated);
            _wrapper.TelemetryUpdated += new EventHandler<SdkWrapper.TelemetryUpdatedEventArgs>(_wrapper_TelemetryUpdated);
            _currentDrivers = new ObservableCollection<DriverModel>();
        }

        private void _wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            SessionInfo = $"{e.TelemetryInfo.TrackTemp.Value.ToString()} C";
            //SessionInfo = e.TelemetryInfo.SessionTimeRemain.Value.ToString();

        }

        private void _wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            //_drivers.Clear();

            DriverInfo = e.SessionInfo["DriverInfo"]["Drivers"]["CarIdx", 0]["UserName"].Value;

            ParseDrivers(e.SessionInfo);

            //foreach (var driver in e.SessionInfo.RawYaml["Drivers"])
            //{
            //    _currentDrivers.Add(driver);
            //    DriverInfo = $"{driver.Name}, {driver.License}";
            //}
        }

        private void ParseDrivers(SessionInfo info)
        {
            int id = 0;
            DriverModel driver = null;

            bool updt = true;

            while (updt)
            {

                driver = null;

                YamlQuery yaml = info["DriverInfo"]["Drivers"]["CarIdx", id];

                string name = yaml["UserName"].GetValue();

                if(name != null)
                {
                    driver = _drivers.FirstOrDefault(d => d.Name == name);

                    if(driver == null)
                    {
                        driver = new DriverModel();
                        driver.Id = id;
                        driver.Name = name;
                        //driver.CarNumber = int.Parse(yaml["CarNumber"].GetValue("").TrimStart('\"').TrimEnd('\"'));
                        driver.Rating = int.Parse(yaml["IRating"].GetValue("0"));
                    }
                }

                _currentDrivers.Add(driver);

                id++;

                updt = false;
            }

        }

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

        private ObservableCollection<DriverModel> _currentDrivers;
        public ObservableCollection<DriverModel> CurrentDrivers
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
