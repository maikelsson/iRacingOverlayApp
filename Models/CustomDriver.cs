using iRacingSimulator.Drivers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingLiveDataOverlay.Models
{
    public class CustomDriver : INotifyPropertyChanged
    {

        private string _name;

        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _position;

        public int Position
        {
            get { return _position; }
            set { _position = value; OnPropertyChanged("Position"); }
        }

        private string _deltaToNext;

        public string DeltaToNext
        {
            get { return _deltaToNext; }
            set { _deltaToNext = value; OnPropertyChanged("DeltaToNext"); }
        }

        public CustomDriver(string name, int position, string delta)
        {
            Name = name;
            Position = position;
            DeltaToNext = delta;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
