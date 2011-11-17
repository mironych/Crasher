using System;
using System.Linq;
using Mono.Android.Crasher.Data;

namespace Mono.Android.Crasher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrasherAttribute : Attribute
    {
        public CrasherAttribute()
        {
            UseCustomData = true;
            ReportContent = null;
        }

        InteractionMode _mode = InteractionMode.Silent;
        string[] _logcatArguments = { "-t", "200", "-v", "time" };

        /// <summary>
        /// The interaction mode you want to implement. Default is InteractionMode.Silent
        /// </summary>
        public InteractionMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        /// Arguments to be passed to the logcat command line. Default is { "-t", "200", "-v", "time" }.
        /// For more info see "http://developer.android.com/intl/fr/guide/developing/tools/adb.html#logcatoptions".
        /// </summary>
        public string[] LogcatArguments
        {
            get { return _logcatArguments; }
            set { _logcatArguments = value; }
        }

        /// <summary>
        /// ReportField Array listing the fields to be included in the report.
        /// </summary>
        public ReportField[] ReportContent
        {
            get;
            set;
        }

        /// <summary>
        /// Add here your SharedPreferences identifier Strings if you use others than your application's default.
        /// They will be added to the ReportField.SharedPreferences field.
        /// </summary>
        public string[] AdditionalSharedPreferences
        {
            get;
            set;
        }

        /// <summary>
        /// Set to true if you want to use some custom report data providers defined in CustomDataProviders.
        /// </summary>
        public bool UseCustomData
        {
            get;
            set;
        }

        private Type[] _customDataProviders;
        /// <summary>
        /// Array of ICustomReportDataProvider implementations to provide custom data for crash report.
        /// Used only if UseCustomData set to true.
        /// </summary>
        public Type[] CustomDataProviders
        {
            get { return _customDataProviders; }
            set
            {
                if (!value.All(type => type.GetInterfaces().Contains(typeof(ICustomReportDataProvider))))
                {
                    throw new ArgumentException("Some of types are not an instance of ICustomReportDataProvider");
                }
                _customDataProviders = value;
            }
        }
    }
}
