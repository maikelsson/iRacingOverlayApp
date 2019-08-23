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
        /// <summary>
        /// Player fullname
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Player ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Player iRating
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Player Car number 
        /// </summary>
        public string CarNumber { get; set; }

        /// <summary>
        /// Current position in session
        /// </summary>
        public int CurrentPosition { get; set; }

        public DriverModel()
        {

        }

    }
}
