using System;
using System.Text;
using Android.Content;
using Android.Util;
using Java.IO;
using Java.Lang;

namespace Mono.Android.Crasher.Utils
{
    class Installation
    {
        private static string _sID;
        private const string INSTALLATION = "Crasher-INSTALLATION";
        private static readonly object _locker = new object();

        public static string Id(Context context)
        {
            if (_sID == null)
            {
                var installation = new File(context.FilesDir, INSTALLATION);
                try
                {
                    if (!installation.Exists())
                    {
                        lock (_locker)
                        {
                            if (!installation.Exists())
                                WriteInstallationFile(installation);
                        }
                    }
                    _sID = ReadInstallationFile(installation);
                }
                catch (IOException e)
                {
                    Log.Warn(Constants.LOG_TAG, "Couldn't retrieve InstallationId for " + context.PackageName, e);
                    return "Couldn't retrieve InstallationId";
                }
                catch (RuntimeException e)
                {
                    Log.Warn(Constants.LOG_TAG, "Couldn't retrieve InstallationId for " + context.PackageName, e);
                    return "Couldn't retrieve InstallationId";
                }
            }
            return _sID;
        }

        private static string ReadInstallationFile(File installation)
        {
            var f = new RandomAccessFile(installation, "r");
            var bytes = new byte[(int)f.Length()];
            try
            {
                f.ReadFully(bytes);
            }
            finally
            {
                f.Close();
            }
            return Encoding.UTF8.GetString(bytes);
        }

        private static void WriteInstallationFile(File installation)
        {
            var o = new FileOutputStream(installation);
            try
            {
                var id = Guid.NewGuid().ToString();
                o.Write(Encoding.UTF8.GetBytes(id));
            }
            finally
            {
                o.Close();
            }
        }
    }
}