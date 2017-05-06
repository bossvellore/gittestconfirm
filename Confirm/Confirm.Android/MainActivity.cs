using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Confirm.Droid;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Media;
using Confirm.Dependency;
using Plugin.Permissions;
using System.Threading.Tasks;
using Android.Webkit;
using Android.Content;
using Android.Locations;

namespace Confirm.Droid
{
	[Activity (Label = "Confirm", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,IGPSChecker
    {
        
        private LocationManager locMgr;
        protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);


            CrossMedia.Current.Initialize();
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
       

            global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new Confirm.App ());

            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        public void OpenGPSSetting()
        {
            Context context = Android.App.Application.Context;

            locMgr = context.GetSystemService(LocationService) as LocationManager;

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

