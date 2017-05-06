using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Confirm.Dependency;
using Confirm.iOS.Dependency;

[assembly: Xamarin.Forms.Dependency(typeof(GetExternalPath))]
namespace Confirm.iOS.Dependency
{
    public class GetExternalPath : IGetExternalStoragePath
    {
        public string GetExternalStoragePath()
        {
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return path;
        }
    }
}