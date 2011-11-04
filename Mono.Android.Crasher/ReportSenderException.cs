using System.Runtime.Serialization;
using Exception = System.Exception;

namespace Mono.Android.Crasher
{
    public class ReportSenderException : Exception
    {
        public ReportSenderException()
        {
        }

        public ReportSenderException(string message) : base(message)
        {
        }

        protected ReportSenderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ReportSenderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}