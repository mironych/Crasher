using Android.Content;
using Android.OS;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Java.Lang;
using StringBuilder = System.Text.StringBuilder;

namespace Mono.Android.Crasher.Utils
{
    class ReportUtils
    {
        public static long GetAvailableInternalMemorySize()
        {
            var path = Environment.DataDirectory;
            var stat = new StatFs(path.Path);
            long blockSize = stat.BlockSize;
            long availableBlocks = stat.AvailableBlocks;
            return availableBlocks * blockSize;
        }

        public static long GetTotalInternalMemorySize()
        {
            var path = Environment.DataDirectory;
            var stat = new StatFs(path.Path);
            var blockSize = stat.BlockSize;
            var totalBlocks = stat.BlockCount;
            return totalBlocks * blockSize;
        }

        public static string GetDeviceId(Context context)
        {
            try
            {
                var tm = TelephonyManager.FromContext(context);
                return tm.DeviceId;
            }
            catch (RuntimeException e)
            {
                Log.Warn(Constants.LOG_TAG, "Couldn't retrieve DeviceId for : " + context.PackageName, e);
                return null;
            }
        }

        public static string GetApplicationFilePath(Context context)
        {
            var filesDir = context.FilesDir;
            if (filesDir != null)
            {
                return filesDir.AbsolutePath;
            }
            Log.Warn(Constants.LOG_TAG, "Couldn't retrieve ApplicationFilePath for : " + context.PackageName);
            return "Couldn't retrieve ApplicationFilePath";
        }

        public static string GetDisplayDetails(Context context)
        {
            try
            {
                var service = context.GetSystemService(Context.WindowService);
                var windowManager = service as IWindowManager;
                if (windowManager == null) return "Could not get WindowManager instance";

                var display = windowManager.DefaultDisplay;
                var metrics = new DisplayMetrics();
                display.GetMetrics(metrics);
                var result = new StringBuilder();
                result.Append("width=").Append(display.Width).Append('\n');
                result.Append("height=").Append(display.Height).Append('\n');
                result.Append("pixelFormat=").Append(display.PixelFormat).Append('\n');
                result.Append("refreshRate=").Append(display.RefreshRate).Append("fps").Append('\n');
                result.Append("metrics.density=x").Append(metrics.Density).Append('\n');
                result.Append("metrics.scaledDensity=x").Append(metrics.ScaledDensity).Append('\n');
                result.Append("metrics.widthPixels=").Append(metrics.WidthPixels).Append('\n');
                result.Append("metrics.heightPixels=").Append(metrics.HeightPixels).Append('\n');
                result.Append("metrics.xdpi=").Append(metrics.Xdpi).Append('\n');
                result.Append("metrics.ydpi=").Append(metrics.Ydpi);
                return result.ToString();

            }
            catch (RuntimeException e)
            {
                Log.Warn(Constants.LOG_TAG, "Couldn't retrieve DisplayDetails for : " + context.PackageName, e);
                return "Couldn't retrieve Display Details";
            }
        }

        public static string GetCrashConfiguration(Context context)
        {
            try
            {
                var crashConf = context.Resources.Configuration;
                return ConfigurationInspector.ToString(crashConf);
            }
            catch (RuntimeException e)
            {
                Log.Warn(Constants.LOG_TAG, "Couldn't retrieve CrashConfiguration for : " + context.PackageName, e);
                return "Couldn't retrieve crash config";
            }
        }
    }
}