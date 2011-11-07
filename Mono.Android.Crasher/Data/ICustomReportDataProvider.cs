using System.Collections.Generic;
using Android.Content;

namespace Mono.Android.Crasher.Data
{
    public interface ICustomReportDataProvider
    {
        IDictionary<string, string> GetReportData(Context context);
    }
}