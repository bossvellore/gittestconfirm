using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Confirm.Dependency;
using Confirm.Droid.Dependency;


[assembly: Xamarin.Forms.Dependency(typeof(GpsEnable))]
namespace Confirm.Droid.Dependency
{
    public class GpsEnable : IGPSChecker
    {
        public void OpenGPSSetting()
        {
          	/**
          	 * No implementation needed here...
          	 * When try to access location it will automatically 
          	 * turn it on, if it is off.
          	 */

        }
    }
}