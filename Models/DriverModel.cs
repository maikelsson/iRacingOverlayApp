using iRacingSimulator.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingLiveDataOverlay
{
    public class DriverModel
    {
        public Driver driver { get; }

        public DriverModel(Driver _driver)
        {
            driver = _driver;
        }

        public string DisplayDriverInfo(Driver driver)
        {
            var result = new StringBuilder();

            result.Append($"{driver.Name}");

            return result.ToString();
        }

    }
}
