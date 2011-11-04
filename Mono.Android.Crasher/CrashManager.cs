using System;
using System.Linq;
using Android.App;
using Android.Util;
using Mono.Android.Crasher.Attributes;
using Mono.Android.Crasher.Data.Submit;

namespace Mono.Android.Crasher
{
    public static class CrashManager
    {
        public static ReportField[] DefaultReportFields = {
                                                              ReportField.ReportID, ReportField.AppVersionCode, ReportField.AppVersionName, ReportField.PackageName,
                                                              ReportField.FilePath, ReportField.PhoneModel, ReportField.Brand, ReportField.Product, ReportField.AndroidVersion, ReportField.Build, ReportField.TotalMemSize, ReportField.AvailableMemSize,
                                                              ReportField.IsSilent, ReportField.StackTrace, ReportField.InitialConfiguration, ReportField.CrashConfiguration, ReportField.Display, ReportField.UserComment,
                                                              ReportField.UserAppStartDate, ReportField.UserCrashDate, ReportField.DumpsysMeminfo, ReportField.Dropbox, ReportField.Logcat, ReportField.Eventslog, ReportField.Radiolog,
                                                              ReportField.DeviceID, ReportField.InstallationID, ReportField.DeviceFeatures, ReportField.Environment, ReportField.SharedPreferences,
                                                              ReportField.SettingsSystem, ReportField.SettingsSecure 
                                                          };
        private static CrasherAttribute _config;
        private static Application _application;

        public static CrasherAttribute Config
        {
            get
            {
                return _config;
            }
        }

        private static ExceptionProcessor _exceptionProcessor;
        internal static ExceptionProcessor ExceptionProcessor
        {
            get { return _exceptionProcessor; }
        }

        public static void Initialize(Application app)
        {
            if (_application != null)
            {
                throw new InvalidOperationException("CrashReporter# init called more than once");
            }

            _application = app;
            _config = app.GetType().GetCustomAttributes(typeof(CrasherAttribute), false).FirstOrDefault() as CrasherAttribute;

            if (_config == null)
            {
                Log.Error(Constants.LOG_TAG, "CrashReporter# init called but no CrasherAttribute on Application class " + _application.PackageName);
                return;
            }

            Log.Debug(Constants.LOG_TAG, "CrasherAttribute is enabled for " + _application.PackageName + ", intializing...");
            _exceptionProcessor = new ExceptionProcessor(_application.ApplicationContext);
        }

        public static void AttachSender<T>(Func<T> valueFactory) where T : class, IReportSender
        {
            if (_application == null || _exceptionProcessor == null)
                throw new InvalidOperationException("Need to call AttachSender method after Initialize");

            //TODO Wait for new version of MonoDroid to use reflection from instantiation
            //var sender = Activator.CreateInstance(typeof(T)) as IReportSender;
            var sender = valueFactory();
            if (sender == null)
                throw new NullReferenceException("Could not create instance of " + typeof(T));
            sender.Initialize(_application);
            _exceptionProcessor.AddReportSender(sender);
        }

        public static void DetachSender<T>(T reporter) where T : IReportSender
        {
            if (_application == null || _exceptionProcessor == null)
                throw new InvalidOperationException("Need to call DetachSender method after Initialize");
            _exceptionProcessor.RemoveReportSender(reporter);
        }

        public static void HandleException(Exception exception)
        {
            if (exception != null)
                HandleException(Java.Lang.Throwable.FromException(exception));
        }

        public static void HandleException(Java.Lang.Throwable tr)
        {
            if (_exceptionProcessor != null)
                _exceptionProcessor.ProcessException(tr);
        }

        public static void Stop()
        {
            if (_exceptionProcessor != null)
                _exceptionProcessor.Dispose();
            _exceptionProcessor = null;
            _config = null;
            _application = null;
        }
    }
}