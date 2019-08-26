using iRacingSimulator.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingLiveDataOverlay.Models
{
    public class MockDriver
    {
        public string Name { get; set; }
        public int IRating { get; set; }
        public int Position { get; set; }

        public MockDriver(string name, int iRating, int position)
        {
            Name = name;
            IRating = iRating;
            Position = position;
        }
    }
}
