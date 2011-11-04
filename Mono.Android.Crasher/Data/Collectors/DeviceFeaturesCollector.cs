using System;
using Android.Content;
using Android.Util;
using System.Text;

namespace Mono.Android.Crasher.Data.Collectors
{
    static class DeviceFeaturesCollector
    {
        public static string GetFeatures(Context ctx)
        {
            if (Compatibility.APILevel < 5)
            {
                return "Data available only with API Level >= 5";
            }
            var result = new StringBuilder();
            try
            {
                foreach (var feature in ctx.PackageManager.GetSystemAvailableFeatures())
                {
                    result.Append(feature.Name);
                    result.AppendLine();
                }
            }
            catch (Exception e)
            {
                Log.Warn(Constants.LOG_TAG, Java.Lang.Throwable.FromException(e), "Couldn't retrieve DeviceFeatures for " + ctx.PackageName);
                result.Append("Could not retrieve data: ");
                result.Append(e.Message);
            }
            return result.ToString();
        }
    }
}