using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;
using System.ComponentModel;
using iRacingSimulator;

namespace iRacingLiveDataOverlay.ViewModels
{

    //Toolbar, settings etc.. create maybe own view for options
    public class HomeViewModel : Screen
    {

        private ShellViewModel _shell;

        private string _connectionBtnStatus = "Start";
        private bool _instanceIsRunning = false;

        public string ConnectionBtnStatus
        {
            get
            {
                return _connectionBtnStatus;
            }
            set
            {
                _connectionBtnStatus = value;
                NotifyOfPropertyChange(() => ConnectionBtnStatus);
            }
        }

        private string _connectionStatus;

        public string ConnectionStatus
        {
            get
            {
                return _connectionStatus;  
            }
            set
            {
                _connectionStatus = value;
                NotifyOfPropertyChange(() => ConnectionStatus);
            }
        }

        public HomeViewModel(ShellViewModel shell)
        {

            _shell = shell;

            //Setting up the simulator, check if running
            if(_instanceIsRunning == false)
            {
                Sim.Instance.Start();
            }
            
            _instanceIsRunning = true;

            Sim.Instance.Connected += OnConnected;
            Sim.Instance.Disconnected += OnDisconnected;
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            CheckSimStatus();
            _instanceIsRunning = false;
        }

        private void OnConnected(object sender, EventArgs e)
        {
            CheckSimStatus();
            _instanceIsRunning = true;
        }

        public void CheckSimStatus()
        {
            if (_instanceIsRunning)
            {
                ConnectionStatus = "Instance is running";
                _connectionBtnStatus = "Stop";
            }
            else
            {
                ConnectionStatus = "Instance not running";
                _connectionBtnStatus = "Start";
            }
        }

        public void ShowLiveData()
        {
            if (_instanceIsRunning)
            {
                ConnectionStatus = "Instance is running";
                _connectionBtnStatus = "Start";
                _shell.OpenLiveDataWindow();
            }
            else
            {
                ConnectionStatus = "Instance not running";
                _connectionBtnStatus = "Stop";
                _shell.CloseLiveDataWindow();


            }
        }
        
    }
}
