using System;
using Android.App;
using Android.Runtime;
using Mono.Android.Crasher;
using Mono.Android.Crasher.Attributes;
using Mono.Android.Crasher.Data.Submit;

namespace CrasherTestApp
{
    [Application]
    [Crasher]
    [GoogleFormReporter("dGtDcHdYUkZQaS15S20tN01XS2FJZkE6MQ")]
    public class CrasherTestApplication : Application
    {
        public CrasherTestApplication(IntPtr doNotUse, JniHandleOwnership transfer)
            : base(doNotUse, transfer)
        {
        }

        public CrasherTestApplication()
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrashManager.Initialize(this);
            CrashManager.AttachSender(() => new GoogleFormSender());
        }
    }
}