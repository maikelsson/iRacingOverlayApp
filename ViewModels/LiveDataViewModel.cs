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

namespace iRacingLiveDataOverlay.ViewModels
{

    //Ingame viewmodel, overlay for iracing
    public class LiveDataViewModel : Screen
    {

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

        private bool _currentlyUpdating = false;

        public LiveDataViewModel()
        {
            _currentDrivers = new ObservableCollection<Driver>();
            Sim.Instance.SessionInfoUpdated += OnSessionInfoUpdated;
            //Sim.Instance.Start();
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
            _currentDrivers.Clear();

            foreach(var driver in Sim.Instance.Drivers)
            {
                if(driver.Live.Position == 0)
                {
                    driver.Live.Position = 99;
                }

                _currentDrivers.Add(driver);
            }
        }

        private void GetTrackTemp(SessionInfo info)
        {
            TrackTemp = Sim.Instance.SessionData.TrackSurfaceTemp;
        }

    }
}
