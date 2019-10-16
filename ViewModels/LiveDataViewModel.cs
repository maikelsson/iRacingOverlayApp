using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;
using iRacingSimulator.Drivers;
using iRacingSimulator;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using iRSDKSharp;
using System.Windows.Media.Animation;
using iRacingLiveDataOverlay.Views;
using iRacingLiveDataOverlay.Models;

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Make LiveDataView visible only when im on track.
        //

        private bool _isCurrentlyUpdating = false;

        private bool _isGreenFlag = false;
        public bool IsGreenFlag
        {
            get
            {
                return _isGreenFlag;
            }
            set
            {
                _isGreenFlag = value;
                OnPropertyChanged("IsGreenFlag");
            }
        }

        private double _elapsedTimeOffset = 0;

        #region SessionData variables


        public string StrenghtOfFieldDisplay { get; set; }

        private string _trackTemp;
        public string TrackTemp
        {
            get
            {
                try
                {
                    return _trackTemp.Trim('C').Trim(' ');
                }
                catch (Exception)
                {
                    return "x";
                }
            }
            set
            {
                _trackTemp = value;
                OnPropertyChanged("TrackTemp");
            }
        }

        public string OffTrackLimit { get; set; }

        private string _currentSessionType;
        public string CurrentSessionType
        {
            get
            {
                return _currentSessionType;
            }
            set
            {
                _currentSessionType = value;
                OnPropertyChanged("CurrentSessionType");
            }
        }

        public string SessionTimeElapsedDisplay
        {
            get
            {
                return ConvertDoubleToTimeString(_sessionTimeElapsed);
            }
        }

        private double _sessionTimeElapsed;
        public double SessionTimeElapsed
        {
            get
            {
                return _sessionTimeElapsed;
            }
            set
            {
                _sessionTimeElapsed = value;
                OnPropertyChanged("SessionTimeElapsed");
                OnPropertyChanged("SessionTimeElapsedDisplay");
            }
        }

        private double _sessionTimeLeft;
        public double SessionTimeLeft
        {
            get
            {
                return _sessionTimeLeft;
            }
            set
            {
                _sessionTimeLeft = value;
                OnPropertyChanged("SessionTimeLeft");
                OnPropertyChanged("SessionTimeLeftDisplay");

            }
        }

        public string SessionTimeLeftDisplay
        {
            get
            {
                return ConvertDoubleToTimeString(_sessionTimeLeft);
            }
        }

        private string _raceTimeDisplay;
        public string RaceTimeDisplay
        {
            get
            {
                return _raceTimeDisplay;
            }
            set
            {
                _raceTimeDisplay = value;
                OnPropertyChanged("RaceTimeDisplay");
            }
        }

        #endregion

        //List of all drivers
        private ObservableCollection<Driver> _allSessionDrivers;
        public ObservableCollection<Driver> AllSessionDrivers
        {
            get
            {
                return _allSessionDrivers;
            }
            set
            {
                _allSessionDrivers = value;
                OnPropertyChanged("AllSessionDrivers");
            }
        }


        private ObservableCollection<CustomDriver> _customList;
        public ObservableCollection<CustomDriver> CustomList
        {
            get
            {
                return _customList;
            }
            set
            {
                _customList = value;
                OnPropertyChanged("CustomList");
            }
        }

        //List of drivers from session results
        private ObservableCollection<Driver> _myClassDrivers;
        public ObservableCollection<Driver> MyClassDrivers
        {
            get
            {
                return _myClassDrivers;
            }
            set
            {
                _myClassDrivers = value;
                //OnPropertyChanged("MyClassDrivers");
            }
        }

        public Driver MyDriver;

        public TelemetryInfo telemetryInfo;

        public LiveDataViewModel()
        {

            Sim.Instance.Start(10);

            CustomList = new ObservableCollection<CustomDriver>();
            AllSessionDrivers = new ObservableCollection<Driver>();
            MyClassDrivers = new ObservableCollection<Driver>();

            Sim.Instance.Connected += OnSimInstanceConnected;
            Sim.Instance.Disconnected += OnSimInstanceDisconnected;

            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            Sim.Instance.TelemetryUpdated += OnTelemetryInfoUpdated;
            Sim.Instance.RaceEvent += OnRaceEventInfoUpdated;
            Sim.Instance.SimulationUpdated += OnSimulationUpdated;

            TelemetryInfo telemetryInfo = Sim.Instance.Telemetry;

        }


        private async void OnSimulationUpdated(object sender, EventArgs e)
        {
            await GetSessionInfo();
        }

        private void OnSimInstanceDisconnected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void OnSimInstanceConnected(object sender, EventArgs e)
        {
            await GetAllDriversFromCurrentSession();
            await GetSessionInfo();
        }

        private void OnRaceEventInfoUpdated(object sender, Sim.RaceEventArgs e)
        {
            //Resetting the clock when the race starts
            if (e.Event.Type.Equals(1))
            {
                IsGreenFlag = true;
                _elapsedTimeOffset = SessionTimeLeft - SessionTimeElapsed;
                Debug.WriteLine(_elapsedTimeOffset);
            }
        }

        private async void OnTelemetryInfoUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            await GetActiveDriversFromCurrentSession(AllSessionDrivers);

            if (IsGreenFlag)
            {
                SessionTimeElapsed = Sim.Instance.SessionData.TimeRemaining + _elapsedTimeOffset;
            }
            else
            {
                SessionTimeElapsed = Sim.Instance.SessionData.TimeRemaining;
            }

        }

        private async void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            await GetAllDriversFromCurrentSession();
            await GetSessionInfo();
        }

        private async Task GetAllDriversFromCurrentSession()
        {
            //Remove items from list to prevent duplicates happening..
            AllSessionDrivers.Clear();
            if (!_isCurrentlyUpdating)
            {

                _isCurrentlyUpdating = true;
                foreach (var driver in Sim.Instance.Drivers)
                {
                    
                    if (driver.IsCurrentDriver)
                    {
                        MyDriver = driver;
                    }

                    AllSessionDrivers.Add(driver);
                }
            }

            _isCurrentlyUpdating = false;
            await GetActiveDriversFromCurrentSession(AllSessionDrivers);
            
        }

        private async Task GetActiveDriversFromCurrentSession(ObservableCollection<Driver> drivers)
        {

            MyClassDrivers.Clear();

            if (!_isCurrentlyUpdating)
            {
                _isCurrentlyUpdating = true;

                foreach (var d in drivers)
                {  
                    if(MyDriver == null)
                    {
                        MyClassDrivers.Add(d);
                    }
                    //Adding driver to standings list only if has completed a valid lap
                    else 
                    {

                        //If driving, add only drivers from same class that has completed atleast one lap
                        if (d.Results.Current.LapsComplete != 0 && d.Car.CarClassId == MyDriver.Car.CarClassId)
                        {
                            MyClassDrivers.Add(d);
                        }  
                    }
                }
            }

            await CalculateSofForMyClass();
            _isCurrentlyUpdating = false;
            await Task.CompletedTask;
        }

        //Get track temps, sessiontype etc. there might be easier or simpler way to do this.. 
        private async Task GetSessionInfo()
        {
            TrackTemp = Sim.Instance.SessionData.TrackSurfaceTemp;
            CurrentSessionType = Sim.Instance.SessionData.SessionType;
            SessionTimeLeft = Sim.Instance.SessionData.RaceTime;
            OffTrackLimit = "sds";
            await Task.CompletedTask;
        }

        private async Task CalculateSofForMyClass()
        {
            var SOF = 0;
            
            foreach(var driver in MyClassDrivers)
            {
                SOF += driver.IRating;
            }

            SOF = SOF / MyClassDrivers.Count();

            StrenghtOfFieldDisplay = SOF.ToString();

            await Task.CompletedTask;
        }

        private string ConvertDoubleToTimeString(double time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            string output = "";
            output = span.ToString(@"mm\:ss");
            return output;
        }

    }

    

}


