using System;
using System.Text;
using Android.Content.Res;
using Android.Util;

namespace Mono.Android.Crasher.Utils
{
    /// <summary>
    /// Inspects a Configuration object through reflection API in order to
    /// generate a human readable String with values replaced with their constants
    /// names. The Configuration.ToString() method was not enough as values
    /// like 0, 1, 2 or 3 don't look readable to me. Using reflection API allows to
    /// retrieve hidden fields and can make us hope to be compatible with all Android
    /// API levels, even those which are not published yet.
    /// </summary>
    class ConfigurationInspector
    {
        /// <summary>
        /// Use this method to generate a human readable String listing all values
        /// from the provided Configuration instance.
        /// </summary>
        /// <param name="conf">Configuration class instance to inspect</param>
        /// <returns>A String describing all the fields of the given Configuration,
        /// with values replaced by constant names.</returns>
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