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
using Confirm.Droid.Dependency;
using Confirm.Dependency;

[assembly: Xamarin.Forms.Dependency(typeof(GetExternalPath))]
namespace Confirm.Droid.Dependency
{
    public class GetExternalPath : IGetExternalStoragePath
    {
        public string GetExternalStoragePath()
        {
            string path = Android.OS.Environment.GetExternalStoragePublicDirectory("").AbsolutePath;
            return path;
        }
    }
}