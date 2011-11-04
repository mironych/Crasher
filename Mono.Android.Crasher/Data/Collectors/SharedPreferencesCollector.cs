﻿using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Preferences;

namespace Mono.Android.Crasher.Data.Collectors
{
    static class SharedPreferencesCollector
    {
        public static string Collect(Context context)
        {
            var result = new StringBuilder();
            var shrdPrefs = new Dictionary<string, ISharedPreferences> { { "default", PreferenceManager.GetDefaultSharedPreferences(context) } };
            var shrdPrefsIds = CrashManager.Config.AdditionalSharedPreferences;
            if (shrdPrefsIds != null && shrdPrefsIds.Length > 0)
            {
                foreach (var shrdPrefId in shrdPrefsIds)
                {
                    shrdPrefs.Add(shrdPrefId, context.GetSharedPreferences(shrdPrefId, FileCreationMode.Private));
                }
            }

            foreach (var prefsId in shrdPrefs.Keys)
            {
                result.AppendLine(prefsId);
                var prefs = shrdPrefs[prefsId];
                if (prefs != null && prefs.All != null && prefs.All.Count > 0)
                {
                    foreach (var p in prefs.All)
                    {
                        result.AppendFormat("{0}={1}", p.Key, p.Value).AppendLine();
                    }
                }
                else
                {
                    result.AppendLine("null");
                }
                result.AppendLine();
            }
            return result.ToString();
        }
    }
}