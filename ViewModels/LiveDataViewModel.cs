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

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Screen
    {
        //TODO's

        //Make LiveDataView visible only when im on track.

        #region SessionData variables

        private string _trackTemp;
        public string TrackTemp
        {
            get
            {
                return _trackTemp;
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
            ParseDynamicInfo();
            GetTrackTemp();
        }

        private void OnTelemetryInfoUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            SessionTimeElapsed = Sim.Instance.SessionData.TimeRemaining;
        }

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            GetTrackTemp();
            ParseDynamicInfo();
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

            UpdateStandings(CurrentDrivers);

        }

        //Updates standing list, top left corner
        private void UpdateStandings(ObservableCollection<Driver> drivers)
        {
            StandingDrivers.Clear();

            foreach(var d in drivers)
            {
                
                //Adding driver to standings list only if has completed a valid lap
                if (d.Results.Current.LapsComplete != 0)
                {
                    StandingDrivers.Add(d);
                }
            }
        }

        //Just to get track temps, there might be easier or simpler way to do this.. 
        private void GetTrackTemp()
        {
            TrackTemp = Sim.Instance.SessionData.TrackSurfaceTemp;
            CurrentSessionType = Sim.Instance.SessionData.SessionType;
            SessionTimeLeft = Sim.Instance.SessionData.SessionTime;
        }

    }

}


