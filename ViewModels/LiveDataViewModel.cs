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

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Screen
    {
        //TODO's

        //Make LiveDataView visible only when im on track.
        //Race clock atm not working correctly.. needs to start when greenflag event is raised.. 

        private bool _isCurrentlyUpdating = false;

        private bool IsGreenFlag = false;

        #region SessionData variables

        private string _trackTemp;
        public string TrackTemp
        {
            get
            {
                return _trackTemp.Trim('C').Trim(' ');
            }
            set
            {
                _trackTemp = value;
                NotifyOfPropertyChange(() => TrackTemp);
            }
        }

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
                NotifyOfPropertyChange(() => CurrentSessionType);
            }
        }

        public string SessionTimeElapsedDisplay
        {
            get
            {
                return ConvertToTime(_sessionTimeElapsed);
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
                NotifyOfPropertyChange(() => SessionTimeElapsed);
                NotifyOfPropertyChange(() => SessionTimeElapsedDisplay);
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
                NotifyOfPropertyChange(() => SessionTimeLeft);
                NotifyOfPropertyChange(() => SessionTimeLeftDisplay);

            }
        }

        public string SessionTimeLeftDisplay
        {
            get
            {
                return ConvertToTime(_sessionTimeLeft);
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
                NotifyOfPropertyChange(() => RaceTimeDisplay);
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
                NotifyOfPropertyChange(() => CurrentDrivers);
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
                NotifyOfPropertyChange(() => StandingDrivers);
            }
        }

        public LiveDataViewModel()
        {
            _currentDrivers = new ObservableCollection<Driver>();
            _standingDrivers = new ObservableCollection<Driver>();
            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            Sim.Instance.TelemetryUpdated += OnTelemetryInfoUpdated;
            Sim.Instance.RaceEvent += OnRaceEventInfoUpdated;
            ParseDynamicInfo();
            GetSessionInfo();
        }

        private void OnRaceEventInfoUpdated(object sender, Sim.RaceEventArgs e)
        {
            if (e.Event.Type.Equals(1))
            {
                IsGreenFlag = true;
            }
        }

        private void OnTelemetryInfoUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            //Todo race clock.
            if (IsGreenFlag)
            {
                SessionTimeElapsed += SessionTimeElapsed - Sim.Instance.SessionData.TimeRemaining;
            }

            SessionTimeElapsed = Sim.Instance.SessionData.TimeRemaining;
            
            UpdateStandings(CurrentDrivers);
        }

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            ParseDynamicInfo();
            GetSessionInfo();
        }

        //Gets all drivers from Session
        private void ParseDynamicInfo()
        {
            //Remove items from list to prevent duplicates happening..
            CurrentDrivers.Clear();

            foreach(var driver in Sim.Instance.Drivers)
            {
                CurrentDrivers.Add(driver);
            }

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
                    //Adding driver to standings list only if has completed a valid lap
                    if (d.Results.Current.LapsComplete != 0)
                    {
                        StandingDrivers.Add(d);
                    }
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
        }

        private string ConvertToTime(double time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            string output = "";

            if (time < 3600)
            {
                output = span.ToString(@"mm\:ss");
                
            }

            else
            {
                output = span.ToString(@"hh\:mm\:ss");
            }
            
            return output;
        }
    }

}


