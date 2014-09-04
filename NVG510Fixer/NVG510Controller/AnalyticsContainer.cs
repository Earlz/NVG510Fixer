using PreEmptive.Analytics.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Earlz.NVG510Controller
{
    public static class AnalyticsContainer
    {
        public static IPlatformClient Client
        {
            get;
            set;
        }
    }
}
