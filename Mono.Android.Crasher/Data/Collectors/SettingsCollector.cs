using System;
using System.Reflection;
using System.Text;
using Android.Content;
using Android.Provider;
using Android.Util;

namespace Mono.Android.Crasher.Data.Collectors
{
    static class SettingsCollector
    {
        public static string CollectSystemSettings(Context ctx)
        {
            var result = new StringBuilder();
            var fields = typeof(Settings.System).GetFields(BindingFlags.Public);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    try
                    {
                        var value = Settings.System.GetString(ctx.ContentResolver, (string)field.GetValue(null));
                        if (value != null)
                        {
                            result.AppendFormat("{0}={1}", field.Name, value).AppendLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Warn(Constants.LOG_TAG, Java.Lang.Throwable.FromException(e), e.Message);
                    }
                }
            }
            return result.ToString();
        }

        public static string CollectSecureSettings(Context ctx)
        {
            var result = new StringBuilder();
            var fields = typeof(Settings.Secure).GetFields(BindingFlags.Public);
            foreach (var field in fields)
            {
                if (field.FieldType != typeof (string)) continue;
                try
                {
                    var value = Settings.Secure.GetString(ctx.ContentResolver, (string)field.GetValue(null));
                    if (value != null)
                    {
                        result.AppendFormat("{0}={1}", field.Name, value).AppendLine();
                    }
                }
                catch (Exception e)
                {
                    Log.Warn(Constants.LOG_TAG, Java.Lang.Throwable.FromException(e), e.Message);
                }
            }
            return result.ToString();
        }

    }
}