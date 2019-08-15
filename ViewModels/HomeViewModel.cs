using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;
using System.ComponentModel;

namespace iRacingLiveDataOverlay.ViewModels
{

    //Toolbar, settings etc.. create maybe own view for options
    public class HomeViewModel : Screen
    {

        private ShellViewModel _shell;

        private string _connectionBtnStatus = "Start";

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

        private readonly SdkWrapper wrapper;

        public HomeViewModel(ShellViewModel shell)
        {
            _shell = shell;
            Services.IRacingService.Initialize();
            wrapper = Services.IRacingService._wrapper;
            //wrapper.SessionInfoUpdated += new EventHandler<SdkWrapper.SessionInfoUpdatedEventArgs>(wrapper_SessionInfoUpdated);
            wrapper.Connected += wrapper_Connected;
            wrapper.Disconnected += wrapper_Disconnected;
        }

        private void wrapper_Disconnected(object sender, EventArgs e)
        {
            CheckWrapperStatus();
            ConnectionBtnStatus = "Stop";
        }

        private void wrapper_Connected(object sender, EventArgs e)
        {
            CheckWrapperStatus();
            ConnectionBtnStatus = "Start";
        }

        //private void wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        public void CheckWrapperStatus()
        {
            if (wrapper.IsConnected)
            {
                if (wrapper.IsRunning)
                {
                    ConnectionStatus = "Connected";
                    _shell.OpenLiveDataWindow();
                }
                else
                {
                    ConnectionStatus = "Disconnected";
                    _shell.CloseLiveDataWindow();
                }
            }
            else
            {
                if (wrapper.IsRunning)
                {
                    ConnectionStatus = $"Disconnected: Waiting for sim";
                }
                else
                {
                    ConnectionStatus = "Disconnected";
                    _shell.CloseLiveDataWindow();
                }
            }      
        }

        public void StartWrapper()
        {
            if (wrapper.IsRunning)
            {
                wrapper.Stop();
            }
            else
            {
                wrapper.Start();
            }

            CheckWrapperStatus();
        }

        
    }
}
