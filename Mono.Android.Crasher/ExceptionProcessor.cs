using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Text.Format;
using Android.Util;
using Java.Lang;
using Mono.Android.Crasher.Data;
using Mono.Android.Crasher.Data.Submit;
using Mono.Android.Crasher.Utils;
using Exception = System.Exception;
using Object = Java.Lang.Object;

namespace Mono.Android.Crasher
{
    class ExceptionProcessor : Object
    {
        private readonly Context _context;
        private readonly List<IReportSender> _reportSenders = new List<IReportSender>();
        private readonly ReportingInteractionMode _reportingInteractionMode;
        private readonly Time _appStartDate;
        private readonly string _initialConfiguration;

        public ExceptionProcessor(Context context)
        {
            _context = context;
            _appStartDate = new Time();
            _appStartDate.SetToNow();
            _reportingInteractionMode = CrashManager.Config.Mode;
            _initialConfiguration = ReportUtils.GetCrashConfiguration(_context);
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironmentUnhandledExceptionRaiser;
        }

        private void AndroidEnvironmentUnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            ProcessException(Throwable.FromException(e.Exception));
        }

        private static readonly object _reportersListLocker = new object();

        public void AddReportSender(IReportSender sender)
        {
            lock (_reportersListLocker)
            {
                if (_reportSenders.Contains(sender)) return;
                _reportSenders.Add(sender);
            }
        }

        public void RemoveReportSender(IReportSender sender)
        {
            lock (_reportersListLocker)
            {
                if (!_reportSenders.Contains(sender)) return;
                _reportSenders.Remove(sender);
            }
        }

        public void ProcessException(Throwable th)
        {
            Log.Error(Constants.LOG_TAG, "Caught a " + th.GetType().Name + " exception for " + _context.PackageName + ". Start building report.");

            var data = ReportDataFactory.BuildReportData(_context, CrashManager.DefaultReportFields, _appStartDate,
                                                         _initialConfiguration, th,
                                                         _reportingInteractionMode == ReportingInteractionMode.Silent);
            Log.Debug(Constants.LOG_TAG, "Start sending report");
            Parallel.ForEach(_reportSenders, s =>
                                                 {
                                                     try
                                                     {
                                                         Log.Debug(Constants.LOG_TAG, "Start sending report by " + s.GetType().Name);
                                                         s.Send(data);
                                                         Log.Debug(Constants.LOG_TAG, "Report was successfully sent by " + s.GetType().Name);
                                                     }
                                                     catch (ReportSenderException e)
                                                     {
                                                         Log.Error(Constants.LOG_TAG, Throwable.FromException(e), e.Message);
                                                     }
                                                     catch (Exception e)
                                                     {
                                                         Log.Error(Constants.LOG_TAG, Throwable.FromException(e),
                                                                   "Unhandled error when sending report with " +
                                                                   s.GetType().FullName);
                                                     }
                                                 });
            Log.Debug(Constants.LOG_TAG, "Report was builded and sent");
        }

        public override void Dispose()
        {
            AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironmentUnhandledExceptionRaiser;
            base.Dispose();
        }
    }
}