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
using iRacingLiveDataOverlay.Models;

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Screen
    {
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

        private SessionInfoModel _sessionInfoModel;

        private bool _currentlyUpdating = false;

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
            if (_currentlyUpdating)
                return;

            _currentlyUpdating = true;

            ParseDrivers(e.SessionInfo);

            _currentlyUpdating = false;
        }

        private void ParseDrivers(SessionInfo info)
        {
            int id = 0;
            DriverModel driver = null;

            var newDrivers = new ObservableCollection<DriverModel>();

            //If username value == null, loop breaks and all drivers have been found
            do
            {

                driver = null;

                YamlQuery yaml = info["DriverInfo"]["Drivers"]["CarIdx", id];

                string name = yaml["UserName"].GetValue();

                if (name != null)
                {
                    driver = _currentDrivers.FirstOrDefault(d => d.Name == name);

                    if (driver == null)
                    {
                        driver = new DriverModel();
                        driver.Id = id;
                        driver.Name = name;
                        driver.CarNumber = "#" + yaml["CarNumber"].GetValue("").TrimStart('\"').TrimEnd('\"');
                        driver.Rating = int.Parse(yaml["IRating"].GetValue("0"));
                    }

                    newDrivers.Add(driver);

                    id++;
                } 
            } while (driver != null);

            _currentDrivers.Clear();
            _currentDrivers = newDrivers;
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

        //private ObservableCollection<DriverModel> _currentDrivers;
        //public ObservableCollection<DriverModel> CurrentDrivers
        //{
        //    get
        //    {
        //        return _currentDrivers;
        //    }
        //    set
        //    {
        //        _currentDrivers = value;
        //        NotifyOfPropertyChange(() => CurrentDrivers);
        //    }
        //}

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
