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

<<<<<<< HEAD
        public ICollection<Driver> ICurrentDrivers;
=======
        public ICollection<Driver> IStandings;
>>>>>>> dev
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

        private bool _currentlyUpdating = false;

        public LiveDataViewModel()
        {
            _currentDrivers = new ObservableCollection<Driver>();
            _standingDrivers = new ObservableCollection<Driver>();
            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            //Sim.Instance.TelemetryUpdated += OnTelemetryInfoUpdated;
        }

        //private void OnTelemetryInfoUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        //{
        //    if (_currentlyUpdating)
        //        return;

        //    _currentlyUpdating = true;

        //    ParseDynamicInfo(e.TelemetryInfo);

        //    _currentlyUpdating = false;
        //}

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {

            GetTrackTemp(e.SessionInfo);
            ParseDynamicInfo(e.SessionInfo);

        }

        private void ParseDynamicInfo(SessionInfo info)
        {
            //Remove items from list to prevent duplicates happening..
            CurrentDrivers.Clear();
<<<<<<< HEAD
            StandingDrivers.Clear();
            ICurrentDrivers.Clear();
=======
>>>>>>> dev

            foreach(var driver in Sim.Instance.Drivers)
            {               
                CurrentDrivers.Add(driver);
            }

            UpdateStandings(CurrentDrivers);

        }

        private void UpdateStandings(ObservableCollection<Driver> drivers)
        {
            StandingDrivers.Clear();

            foreach (var d in drivers)
            {
                if(d.Live.Position != 0)
                {
                    StandingDrivers.Add(d); 
                }
            }

<<<<<<< HEAD
            ICurrentDrivers = this.CurrentDrivers;
            ICurrentDrivers.OrderBy(p => p.Live.Position);
=======
            SortDrivers(StandingDrivers);

>>>>>>> dev
        }

        //private void GetLiveStandings(SessionInfo info)
        //{
        //    var id = 0;
        //    var query = info[];

        //    foreach(var driver in Sim.Instance.SessionInfo.GetValue(""))
        //    {
        //        var d = CurrentDrivers.Where(c => c.Car. == id);
        //    }
        //}

        private void GetTrackTemp(SessionInfo info)
        {
            TrackTemp = Sim.Instance.SessionData.TrackSurfaceTemp;
        }

        //Sorting for observablecollections with type Driver, modify to accept other types maybe
        public static ObservableCollection<Driver> SortDrivers(ObservableCollection<Driver> drivers)
        {
            ObservableCollection<Driver> sorted;
            sorted = new ObservableCollection<Driver>(drivers.OrderBy(p => p.Live.Position));
            drivers.Clear();
            
            foreach(Driver d in sorted)
            {
                drivers.Add(d);
            }

            return drivers;

        }

    }

}


