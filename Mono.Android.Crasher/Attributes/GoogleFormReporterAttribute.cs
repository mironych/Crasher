using System;

namespace Mono.Android.Crasher.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GoogleFormReporterAttribute : Attribute
    {
        public string FormKey { get; set; }
        public int ConnectionTimeout { get; set; }

        public GoogleFormReporterAttribute(string formKey, int connectionTimeout=10000)
        {
            FormKey = formKey;
            ConnectionTimeout = connectionTimeout;
        }
    }
}