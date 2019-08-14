using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSdkWrapper;

namespace iRacingLiveDataOverlay.Services
{
    public static class IRacingService
    {
        public static SdkWrapper _wrapper;

        public static void Initialize()
        {
            _wrapper = new SdkWrapper();
            _wrapper.EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread;
        }

    }
}
