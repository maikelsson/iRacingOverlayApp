using iRacingSimulator.Drivers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using iRacingLiveDataOverlay.Models;

namespace iRacingLiveDataOverlay.ViewModels
{
    public class MockLiveDataViewModel : Screen
    {
        private ObservableCollection<MockDriver> _mockDrivers;
        public ObservableCollection<MockDriver> MockDrivers
        {
            get
            {
                return _mockDrivers;
            }
            set
            {
                _mockDrivers = value;
                NotifyOfPropertyChange(() => MockDrivers);
            }
        }

        public MockLiveDataViewModel()
        {
            MockDrivers = new ObservableCollection<MockDriver>();
            LoadMockDrivers();
        }

        private void LoadMockDrivers()
        {  

            MockDriver driver1 = new MockDriver("Mikael Thornberg", 1200, 3);
            MockDriver driver2 = new MockDriver("Test Driver", 5630, 1);
            MockDriver driver3 = new MockDriver("Kimi Räikkönen", 7600, 2);
            MockDriver driver4 = new MockDriver("Jukka Haapala", 700, 4);
            MockDriver driver5 = new MockDriver("Matti Riuska", 2345, 5);

            MockDrivers.Add(driver1);
            MockDrivers.Add(driver2);
            MockDrivers.Add(driver3);
            MockDrivers.Add(driver4);
            MockDrivers.Add(driver5);
        }
    }
}
