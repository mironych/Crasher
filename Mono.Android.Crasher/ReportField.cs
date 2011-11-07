namespace Mono.Android.Crasher
{
    public enum ReportField
    {
        /**
        * Report Identifier
        */
        ReportID,
        /**
         * Application version code. This is the incremental integer version code
         * used to differentiate versions on the android market.
         * @see android.content.pm.PackageInfo#versionCode
         */
        AppVersionCode,
        /**
         * Application version name.
         * @see android.content.pm.PackageInfo#versionName
         */
        AppVersionName,
        /**
         * Application package name.
         * @see android.content.Context#getPackageName()
         */
        PackageName,
        /**
         * Base path of the application's private file folder.
         * @see android.content.Context#getFilesDir()
         */
        FilePath,
        /**
         * Device model name.
         * @see android.os.Build#MODEL
         */
        PhoneModel,
        /**
         * Device android version name.
         * @see android.os.Build.VERSION#RELEASE
         */
        AndroidVersion,
        /**
         * Android Build details.
         * @see android.os.Build
         */
        Build,
        /**
         * Device brand (manufacturer or carrier).
         * @see android.os.Build#BRAND
         */
        Brand,
        /**
         * Device overall product code.
         * @see android.os.Build#PRODUCT
         */
        Product,
        /**
         * Estimation of the total device memory size based on filesystem stats.
         */
        TotalMemSize,
        /**
         * Estimation of the available device memory size based on filesystem stats.
         */
        AvailableMemSize,
        /**
         * The Holy Stack Trace.
         */
        StackTrace,
        /**
         * {@link Configuration} fields state on the application start.
         * @see Configuration
         */
        InitialConfiguration,
        /**
         * {@link Configuration} fields state on the application crash.
         * @see Configuration
         */
        CrashConfiguration,
        /**
         * Device display specifications.
         * @see android.view.WindowManager#getDefaultDisplay()
         */
        Display,
        /**
         * Comment added by the user in the CrashReportDialog displayed in
         * {@link ReportingInteractionMode#NOTIFICATION} mode.
         */
        UserComment,
        /**
         * User date on application start.
         */
        UserAppStartDate,
        /**
         * User date immediately after the crash occurred.
         */
        UserCrashDate,
        /**
         * Memory state details for the application process.
         */
        DumpsysMeminfo,
        /**
         * Logcat default extract. Requires READ_LOGS permission.
         */
        Logcat,
        /**
         * Logcat eventslog extract. Requires READ_LOGS permission.
         */
        Eventslog,
        /**
         * Logcat radio extract. Requires READ_LOGS permission.
         */
        Radiolog,
        /**
         * True if the report has been explicitly sent silently by the developer.
         */
        IsSilent,
        /**
         * Device unique ID (IMEI). Requires READ_PHONE_STATE permission.
         */
        DeviceID,
        /**
         * Installation unique ID. This identifier allow you to track a specific
         * user application installation without using any personal data.
         */
        InstallationID,
        /**
         * Features declared as available on this device by the system.
         */
        DeviceFeatures,
        /**
         * External storage state and standard directories.
         */
        Environment,
        /**
         * System settings.
         */
        SettingsSystem,
        /**
         * Secure settings (applications can't modify them).
         */
        SettingsSecure,
        /**
         * SharedPreferences contents
         */
        SharedPreferences
    }
}