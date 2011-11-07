using System.Collections.Generic;
using Android.Content;
using Mono.Android.Crasher.Data;

namespace CrasherTestApp
{
    public class TestAppCustomDataReportProvider : ICustomReportDataProvider
    {
        public IDictionary<string, string> GetReportData(Context context)
        {
            return new Dictionary<string, string>
                       {
                           {"CustomReportProvider","Data from TestAppCustomDataReportProvider"}
                       };
        }
    }
}