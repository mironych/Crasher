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

        public InteractionMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public string[] LogcatArguments
        {
            get { return _logcatArguments; }
            set { _logcatArguments = value; }
        }

        public ReportField[] ReportContent
        {
            get;
            set;
        }

        public string[] AdditionalSharedPreferences
        {
            get;
            set;
        }

        public bool UseCustomData
        {
            get;
            set;
        }

        private Type[] _customDataProviders;
        public Type[] CustomDataProviders
        {
            get { return _customDataProviders; }
            set
            {
                if (!value.All(type => type.GetInterfaces().Contains(typeof(ICustomReportDataProvider))))
                {
                    throw new ArgumentException("Type is not instance of ICustomReportDataProvider");
                }
                _customDataProviders = value;
            }
        }
    }
}
