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
            if (Compatibility.APILevel < 7)
            {
                return "Data available only with API Level >= 7";
            }
            var result = new StringBuilder();
            try
            {
                var getSystemAvailableFeaturesMethod =
                    ctx.PackageManager.GetType().GetMethod("GetSystemAvailableFeatures");
                if (getSystemAvailableFeaturesMethod == null) return string.Empty;
                var features = (object[])getSystemAvailableFeaturesMethod.Invoke(ctx.PackageManager, null);
                foreach (var feature in features)
                {
                    var nameProperty = feature.GetType().GetProperty("Name");
                    if (nameProperty == null) continue;
                    result.Append(nameProperty.GetValue(feature, null)).AppendLine();
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