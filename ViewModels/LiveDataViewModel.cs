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
using System.Globalization;

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Screen
    {
        //private SessionInfo _sessionInfo;
        //public SessionInfo SessionInfo
        //{
        //    get
        //    {
        //        return _sessionInfo;
        //    }
        //    set
        //    {
        //        _sessionInfo = value;
        //        NotifyOfPropertyChange(() => SessionInfo);
        //    }
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

        private int _currentSessionNum;
        private bool _currentlyUpdating = false;

        private readonly SdkWrapper _wrapper;

        public LiveDataViewModel()
        {
            _wrapper = IRacingService._wrapper;
            _wrapper.TelemetryUpdateFrequency = 10;
            _wrapper.SessionInfoUpdated += new EventHandler<SdkWrapper.SessionInfoUpdatedEventArgs>(_wrapper_SessionInfoUpdated);
            _wrapper.TelemetryUpdated += new EventHandler<SdkWrapper.TelemetryUpdatedEventArgs>(_wrapper_TelemetryUpdated);
            _currentDrivers = new ObservableCollection<Driver>();
        }


        private void _wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            if (_currentlyUpdating) return;

            // Store the current session number so we know which session to read 
            // There can be multiple sessions in a server (practice, Q, race, or warmup, race, etc).
            _currentSessionNum = e.TelemetryInfo.SessionNum.Value;
            //SessionInfo = $"{e.TelemetryInfo.TrackTemp.Value.ToString()} C";
            //SessionInfo = e.TelemetryInfo.SessionTimeRemain.Value.ToString();

        }

        private void _wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {

            //ParseDrivers(e.SessionInfo);
            //ParseTimes(e.SessionInfo);

            if (_currentlyUpdating)
                return;

            _currentlyUpdating = true;

            ParseDynamicInfo(e.SessionInfo);

            _currentlyUpdating = false;
        }

        private void ParseDynamicInfo(SessionInfo info)
        {
            Driver _driver = null;

            if (_currentlyUpdating)
                return;

            foreach(var driver in Sim.Instance.Drivers)
            {
                _currentDrivers.Add(driver);
            }
        }

        

        private void ParseTimes(SessionInfo info)
        {
            int position = 1;
            Driver driver;

            do
            {
                driver = null;

                YamlQuery query = info["SessionInfo"]["Sessions"]["SessionNum", _currentSessionNum]
                    ["ResultsPositions"]["Position", position];

                string idString = query["CarIdx"].GetValue();
                if(idString != null)
                {
                    int id = int.Parse(idString);

                    driver = _currentDrivers.FirstOrDefault(d => d.Id == id);

                    DriverResults driverResults = new DriverResults(driver);

                    if (driver != null || driverResults.HasResult(_currentSessionNum))
                    {
                        var session = driverResults.FromSession(_currentSessionNum);
                        
                    }

                    position++;
                }
                
            } while (driver != null); 
        }

        private void UpdateDriversTelemetry(TelemetryInfo info)
        {

        }

        private void ParseDrivers(SessionInfo info)
        {

            if (_currentDrivers.Count() > 0)
                _currentDrivers.Clear();

            int id = 0;
            Driver driver = null;

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
                        driver = new Driver();
                        driver.Id = id;
                        driver.Name = name;
                        //driver.LongDisplay;
                        driver.IRating = int.Parse(yaml["IRating"].GetValue("0"));
                    }

                    _currentDrivers.Add(driver);

                    id++;
                } 
            } while (driver != null);

        }

        

    }
}
