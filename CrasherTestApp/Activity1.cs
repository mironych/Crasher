using System;
using Android.App;
using Android.Widget;
using Android.OS;

namespace CrasherTestApp
{
    [Activity(Label = "CrasherTestApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += delegate
                                {
                                    throw new NullReferenceException("Testing  exception handling");
                                };
        }
    }
}

