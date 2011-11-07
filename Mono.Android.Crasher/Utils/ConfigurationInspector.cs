using System;
using System.Text;
using Android.Content.Res;
using Android.Util;

namespace Mono.Android.Crasher.Utils
{
    class ConfigurationInspector
    {
        public static string ToString(Configuration conf)
        {
            var result = new StringBuilder();
            try
            {
                foreach (var p in typeof(Configuration).GetProperties())
                {
                    if (p.Name.Equals("class", StringComparison.OrdinalIgnoreCase) || p.Name.Equals("handle", StringComparison.OrdinalIgnoreCase))
                        continue;
                    result.AppendFormat("{0}={1}", p.Name, p.GetValue(conf, null)).AppendLine();
                }
            }
            catch (Exception e)
            {
                Log.Error(Constants.LOG_TAG, Java.Lang.Throwable.FromException(e), "Error while inspecting device configuration");
            }
            return result.ToString();
        }
    }
}