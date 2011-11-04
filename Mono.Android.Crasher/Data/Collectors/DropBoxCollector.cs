using Android.Content;
using Android.Text.Format;
using Android.Util;
using System.Collections.Generic;
using System.Text;
using System;

namespace Mono.Android.Crasher.Data.Collectors
{
    static class DropBoxCollector
    {
        private static readonly string[] _systemTags = { "system_app_anr", "system_app_wtf", "system_app_crash",
            "system_server_anr", "system_server_wtf", "system_server_crash", "BATTERY_DISCHARGE_INFO",
            "SYSTEM_RECOVERY_LOG", "SYSTEM_BOOT", "SYSTEM_LAST_KMSG", "APANIC_CONSOLE", "APANIC_THREADS",
            "SYSTEM_RESTART", "SYSTEM_TOMBSTONE", "data_app_strictmode" };

        private const string NO_RESULT = "N/A";

        public static string Read(Context context, string[] additionalTags)
        {
            try
            {
                // Use reflection API to allow compilation with API Level 5.
                var serviceName = Compatibility.DropBoxServiceName;
                if (serviceName == null)
                {
                    return NO_RESULT;
                }

                var dropbox = context.GetSystemService(serviceName);
                var getNextEntry = dropbox.GetType().GetMethod("GetNextEntry", new[] { typeof(string), typeof(long) });
                if (getNextEntry == null)
                {
                    return "";
                }

                var timer = new Time();
                timer.SetToNow();
                timer.Minute -= CrashManager.Config.DropBoxCollectionMinutes;
                timer.Normalize(false);
                var time = timer.ToMillis(false);
                var tags = new List<string>();
                if (CrashManager.Config.IncludeDropBoxSystemTags)
                {
                    tags.AddRange(_systemTags);
                }
                if (additionalTags != null && additionalTags.Length > 0)
                {
                    tags.AddRange(additionalTags);
                }

                if (tags.Count == 0)
                {
                    return "No tag configured for collection.";
                }

                var dropboxContent = new StringBuilder();
                foreach (var tag in tags)
                {
                    dropboxContent.Append("Tag: ").Append(tag).AppendLine();
                    var entry = getNextEntry.Invoke(dropbox, new object[] { tag, time });
                    if (entry == null)
                    {
                        dropboxContent.Append("Nothing.").AppendLine();
                        continue;
                    }

                    var getText = entry.GetType().GetMethod("GetText", new[] { typeof(int) });
                    var getTimeMillis = entry.GetType().GetMethod("GetTimeMillis");
                    var close = entry.GetType().GetMethod("Close");
                    while (entry != null)
                    {
                        var msec = (long)getTimeMillis.Invoke(entry, null);
                        timer.Set(msec);
                        dropboxContent.Append("@").Append(timer.Format2445()).AppendLine();
                        var text = (string)getText.Invoke(entry, new object[] { 500 });
                        if (text != null)
                        {
                            dropboxContent.Append("Text: ").Append(text).AppendLine();
                        }
                        else
                        {
                            dropboxContent.Append("Not Text!").AppendLine();
                        }
                        close.Invoke(entry, null);
                        entry = getNextEntry.Invoke(dropbox, new object[] { tag, msec });
                    }
                }
                return dropboxContent.ToString();

            }
            catch (Exception e)
            {
                Log.Info(Constants.LOG_TAG, Java.Lang.Throwable.FromException(e), "DropBoxManager not available.");
            }
            return NO_RESULT;
        }
    }
}