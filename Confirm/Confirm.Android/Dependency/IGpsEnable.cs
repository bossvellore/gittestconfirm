using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Confirm.Dependency;
using Confirm.Droid.Dependency;
using Android.Locations;

[assembly: Xamarin.Forms.Dependency(typeof(IGpsEnable))]
namespace Confirm.Droid.Dependency
{
    public class IGpsEnable : IGPSChecker
    {
        public void OpenGPSSetting()
        {
          LocationManager locMgr;
            Context context = Android.App.Application.Context;

            locMgr = context.GetSystemService("location") as LocationManager;

            bool isGPSEnabled = locMgr.IsProviderEnabled(LocationManager.GpsProvider);

            if (!isGPSEnabled)
            {
                var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                intent.SetFlags(ActivityFlags.NewTask);
                context.StartActivity(intent);
            }
        }
    }
}