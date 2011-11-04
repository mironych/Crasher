using Android.App;

namespace Mono.Android.Crasher.Data.Submit
{
    public interface IReportSender
    {
        void Initialize(Application application);
        void Send(ReportData errorContent);
    }
}