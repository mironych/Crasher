using System.Collections.Generic;
using System.IO;
using Android.Util;
using System.Text;

namespace Mono.Android.Crasher.Data.Collectors
{
    static class LogCatCollector
    {
        private const int DEFAULT_TAIL_COUNT = 100;

        public static string CollectLogCat(string bufferName)
        {
            var commandLine = new List<string> { "logcat" };
            if (bufferName != null)
            {
                commandLine.Add("-b");
                commandLine.Add(bufferName);
            }

            int tailCount;
            var logcatArgumentsList = new List<string>(CrashManager.Config.LogcatArguments);
            var tailIndex = logcatArgumentsList.IndexOf("-t");
            if (tailIndex > -1 && tailIndex < logcatArgumentsList.Count)
            {
                tailCount = int.Parse(logcatArgumentsList[tailIndex + 1]);
                if (Compatibility.APILevel < 8)
                {
                    logcatArgumentsList.RemoveAt(tailIndex + 1);
                    logcatArgumentsList.RemoveAt(tailIndex);
                    logcatArgumentsList.Add("-d");
                }
            }
            else
            {
                tailCount = -1;
            }

            var logcatBuf = new StringBuilder(tailCount > 0 ? tailCount : DEFAULT_TAIL_COUNT);
            commandLine.AddRange(logcatArgumentsList);
            try
            {
                using (var process = Java.Lang.Runtime.GetRuntime().Exec(commandLine.ToArray()))
                {
                    using (var reader = new StreamReader(process.InputStream))
                    {
                        Log.Debug(Constants.LOG_TAG, "Retrieving logcat output...");
                        logcatBuf.AppendLine(reader.ReadToEnd());
                        reader.Close();
                    }
                    process.Destroy();
                }
            }
            catch (IOException e)
            {
                Log.Error(Constants.LOG_TAG, Java.Lang.Throwable.FromException(e), "LogCatCollector.collectLogCat could not retrieve data.");
            }
            return logcatBuf.ToString();
        }
    }
}