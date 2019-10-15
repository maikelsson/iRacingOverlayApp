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

        //TODO's

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
                OnPropertyChanged("CurrentDrivers");
            }
        }

        //List of drivers from session results
        private ObservableCollection<Driver> _standingDrivers;
        public ObservableCollection<Driver> StandingDrivers
        {
            get
            {
                return _standingDrivers;
            }
            set
            {
                _standingDrivers = value;
                OnPropertyChanged("StandingDrivers");
            }
        }

        public TelemetryInfo telemetryInfo;

        public Driver myDriver;

        public LiveDataViewModel()
        {

            Sim.Instance.Start(1);
               
            _currentDrivers = new ObservableCollection<Driver>();
            _standingDrivers = new ObservableCollection<Driver>();

            Sim.Instance.Connected += OnSimInstanceConnected;
            Sim.Instance.Disconnected += OnSimInstanceDisconnected;

            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            Sim.Instance.TelemetryUpdated += OnTelemetryInfoUpdated;
            Sim.Instance.RaceEvent += OnRaceEventInfoUpdated;

        }

        private void OnSimInstanceDisconnected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnSimInstanceConnected(object sender, EventArgs e)
        {
            GetAllDrivers();
            GetSessionInfo();
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

        private void OnTelemetryInfoUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            
            UpdateStandings(CurrentDrivers);

            if (IsGreenFlag)
            {
                SessionTimeElapsed = Sim.Instance.SessionData.TimeRemaining + _elapsedTimeOffset;
            }
            else
            {
                SessionTimeElapsed = Sim.Instance.SessionData.TimeRemaining;
            }
            
        }

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            GetAllDrivers();
            GetSessionInfo();
        }

        //Gets all drivers from the current Session
        private void GetAllDrivers()
        {
            //Remove items from list to prevent duplicates happening..
            CurrentDrivers.Clear();

            if (!_isCurrentlyUpdating)
            {
                _isCurrentlyUpdating = true;

                foreach (var driver in Sim.Instance.Drivers)
                {
                    if (driver.IsCurrentDriver)
                    {
                        myDriver = driver;
                    }

                    CurrentDrivers.Add(driver);
                }
            }

            _isCurrentlyUpdating = false;

        }

        //Updates standing list, top left corner
        private void UpdateStandings(ObservableCollection<Driver> drivers)
        {

            StandingDrivers.Clear();

            if (!_isCurrentlyUpdating)
            {
                _isCurrentlyUpdating = true;

                foreach (var d in drivers)
                {
                    
                    if(myDriver == null)
                    {
                        StandingDrivers.Add(d);
                    }
                    //Adding driver to standings list only if has completed a valid lap
                    else 
                    {
                        if (d.Results.Current.LapsComplete != 0 && d.Car.CarClassId == myDriver.Car.CarClassId)
                        {
                            StandingDrivers.Add(d);
                        }  
                    }

                    StandingDrivers.OrderBy(w => w.Live.ClassPosition);
                }
            }

            _isCurrentlyUpdating = false;
            
        }

        //Get track temps, sessiontype etc. there might be easier or simpler way to do this.. 
        private void GetSessionInfo()
        {
            TrackTemp = Sim.Instance.SessionData.TrackSurfaceTemp;
            CurrentSessionType = Sim.Instance.SessionData.SessionType;
            SessionTimeLeft = Sim.Instance.SessionData.RaceTime;
            OffTrackLimit = "sds";
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


