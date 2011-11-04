using System;

namespace Mono.Android.Crasher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrasherAttribute : Attribute
    {
        ReportingInteractionMode _mode = ReportingInteractionMode.Silent;
        int _dropboxCollectionMinutes = 5;
        string[] _logcatArguments = { "-t", "200", "-v", "time" };

        public bool IncludeDropBoxSystemTags
        {
            get;
            set;
        }

        public string[] AdditionalDropBoxTags
        {
            get;
            set;
        }

        public ReportingInteractionMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public int DropBoxCollectionMinutes
        {
            get { return _dropboxCollectionMinutes; }
            set { _dropboxCollectionMinutes = value; }
        }

        public string[] LogcatArguments
        {
            get { return _logcatArguments; }
            set { _logcatArguments = value; }
        }

        public ReportField[] CustomReportContent
        {
            get;
            set;
        }

        public string[] AdditionalSharedPreferences { get; set; }
    }
}
