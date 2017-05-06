using Confirm.AzureCloudManager;
using Confirm.Dependency;
using Confirm.Pages;
using Confirm.Utils;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Confirm
{
	public partial class App : Application
	{
     
        
        public App ()
		{
			InitializeComponent();
            
            MainPage = new NavigationPage(new ConfirmTabbedPage());
        }
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}


      
       
    }
}
