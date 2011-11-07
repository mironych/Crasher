using Android.Content;
using Android.Content.PM;
using Android.Util;
using Java.Lang;

namespace Mono.Android.Crasher.Utils
{
    class PackageManagerWrapper
    {
        private readonly Context _context;

        public PackageManagerWrapper(Context context)
        {
            _context = context;
        }

        public bool HasPermission(string permission)
        {
            var pm = _context.PackageManager;
            if (pm == null)
            {
                return false;
            }

            try
            {
                return pm.CheckPermission(permission, _context.PackageName) == Permission.Granted;
            }
            catch (RuntimeException e)
            {
                return false;
            }
        }

        public PackageInfo GetPackageInfo()
        {
            var pm = _context.PackageManager;
            if (pm == null)
            {
                return null;
            }

            try
            {
                return pm.GetPackageInfo(_context.PackageName, 0);
            }
            catch (PackageManager.NameNotFoundException e)
            {
                Log.Debug(Constants.LOG_TAG, "Failed to find PackageInfo for current App : " + _context.PackageName);
                return null;
            }
            catch (RuntimeException e)
            {
                return null;
            }
        }
    }
}