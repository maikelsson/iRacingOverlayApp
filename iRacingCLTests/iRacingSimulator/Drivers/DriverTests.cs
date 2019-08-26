using Microsoft.VisualStudio.TestTools.UnitTesting;
using iRacingSimulator.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSimulator.Drivers.Tests
{
    [TestClass()]

    public class DriverTests
    {

        [TestMethod()]
        public void GetShortIRatingTest()
        {
            Driver driver = new Driver();
            driver.IRating = 4900;
            string expected = "4.9k";

            var actual = driver.GetShortIRating(driver.IRating);

            Assert.AreEqual(expected, actual);
        }
    }
}