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

        public ICollection<Driver> ICurrentDrivers;
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
        private List<Driver> _standingDrivers;
        public List<Driver> StandingDrivers
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
            CurrentDrivers = new ObservableCollection<Driver>();
            StandingDrivers = new List<Driver>();
            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            //Sim.Instance.TelemetryUpdated += OnTelemetryInfoUpdated;
        }

        //private void OnTelemetryInfoUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void OnSessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            if (_currentlyUpdating)
                return;

            _currentlyUpdating = true;

            GetTrackTemp(e.SessionInfo);
            ParseDynamicInfo(e.SessionInfo);

            _currentlyUpdating = false;
        }

        private void ParseDynamicInfo(SessionInfo info)
        {
            //Remove items from list to prevent duplicates happening..
            CurrentDrivers.Clear();
            StandingDrivers.Clear();
            ICurrentDrivers.Clear();

            foreach(var driver in Sim.Instance.Drivers)
            {               
                CurrentDrivers.Add(driver);
                if(driver.Live.Position != 0)
                {
                    StandingDrivers.Add(driver);
                }
            }

            ICurrentDrivers = this.CurrentDrivers;
            ICurrentDrivers.OrderBy(p => p.Live.Position);
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

    }
}
