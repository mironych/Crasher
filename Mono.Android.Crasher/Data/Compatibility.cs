using Android.Content;
using Android.OS;

namespace Mono.Android.Crasher.Data
{
    class Compatibility
    {
        public static int APILevel
        {
            get { return Build.VERSION.SdkInt; }
        }

        public static string DropBoxServiceName
        {
            get
            {
                var serviceName = typeof(Context).GetField("DropboxService");
                return serviceName == null ? string.Empty : serviceName.GetValue(null) as string;
            }
        }
    }
}