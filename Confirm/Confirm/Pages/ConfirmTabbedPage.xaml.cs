using Confirm.AzureCloudManager;
using Confirm.Dependency;
using Confirm.Model;
using Confirm.Pages;
using Confirm.Utils;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;


namespace Confirm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmTabbedPage : TabbedPage 
    {
        ConfirmTableManager _tableManager;
        InputValidator _inputValidator;
        MediaFile uploadFile;
        ConfirmRecord confirmRecord;
        string imageName = "";

        //Brand Selection 
        bool isBrand1Selected = false;
        bool isBrand2Selected = false;
        bool isBrand3Selected = false;

        
        string selectedCampaignID = "";

        List<Campaign> campaignsList;
        List<Brand> brandsList;
        public ConfirmTabbedPage ()
        {
            InitializeComponent();

            _tableManager = new ConfirmTableManager();
            _inputValidator = new InputValidator();
            confirmRecord = new ConfirmRecord();
            campaignsList = new List<Campaign>();
            brandsList = new List<Brand>();
            

            Sync();
            OnLogRecordButtonClicked();
            BindListViewContents();

            var dependency = DependencyService.Get<IGetExternalStoragePath>();
            Debug.WriteLine("External Storage : " + dependency.GetExternalStoragePath());


        }
        private void OnGPSButtonClicked(object sender,EventArgs e)
        {
            DependencyService.Get<IGPSChecker>().OpenGPSSetting();
        }

        private void OnLogOutClicked(object sender, EventArgs e)
        {
            
        }
        private async void BindListViewContents()
        {
            List<ConfirmRecord> cR = await _tableManager.GetAllLocalConfirmRecords();
            Name_ListView.ItemsSource = cR;
        }
        private async void Sync()
        {
            await _tableManager.SyncLocalDBWithAzureDB() .ContinueWith( (T) =>
            {
                if (T.Status == TaskStatus.RanToCompletion)
                {
                    Debug.WriteLine("RanToCompletion");
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Debug.WriteLine("Sync Error");
                        BindAllCampaigns();
                    });
                }
                else
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        BindAllCampaigns();
                    });
            });
            
        }

        private async void OnCaptureButtonClicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
            imageName = "ConfirmImage" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            uploadFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                Directory = "ConfirmAPP",
                Name = imageName
            });
            if (uploadFile == null)
            {
                imageName = "";
                return;
            }


            previewImage.Source = ImageSource.FromFile(uploadFile.Path);
            //Name_Label_NoImages.IsVisible = false;
            Name_ImagePlaceHolder.IsVisible = false;
            previewImage.IsVisible = true;
            Debug.WriteLine("Image File Path : " + uploadFile.Path);

            confirmRecord.ImageName = imageName;
        }

        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {

            double lat = 0;
            double lng = 0;

            ConfirmRecord confirmRecord = new ConfirmRecord
            {
                ActiveCampaign = selectedCampaignID,
                Brand1 = ReturnBrandID(0, isBrand1Selected),
                Brand2 = ReturnBrandID(1, isBrand2Selected),
                Brand3 = ReturnBrandID(2, isBrand3Selected),
                StoreInformation = Name_EntryStoreInfo.Text,
                Note = Name_EntryNote.Text,
                Latitude = lat,
                Longitude = lng,
                ImageName = imageName,
                CreatedDate = DateTime.Now.ToString("MM/dd/yyyy h:mm tt")
            };

            bool isValid = false;

            List<bool> validationResult = (_inputValidator.ValidationResult(confirmRecord, out isValid));
            DisplayAlert(validationResult);

            if (isValid)
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    if (!locator.IsGeolocationEnabled)
                    {
                        var isOpenSettingsClicked = await DisplayAlert("Alert", "GPS is disable in your device. Please enable GPS to submit record", "Go to settings", "No");
                       
                        if(isOpenSettingsClicked)
                        {
                            var dependency = DependencyService.Get<IGPSChecker>();
                            dependency.OpenGPSSetting();
                        }
                    }
                    else if(locator.IsGeolocationEnabled)
                    { 

                    locator.DesiredAccuracy = 50;

                    var position = await locator.GetPositionAsync(100000);
                    if (position == null)
                        return;
                    lat = position.Latitude;
                    lng = position.Longitude;

                    confirmRecord.Latitude = lat;
                    confirmRecord.Longitude = lng;

                    await _tableManager.UpsertRecord(confirmRecord);
                    await DisplayAlert("Success", "Your response has been saved successfully", "Ok, create new response");
                    ResetUI();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", "Unable to get location, Please submit record again", "Ok");
                    Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                }
                
            }
            else
            {
                // DisplayAlert(validationResult);
            }

            OnLogRecordButtonClicked();
            BindListViewContents();

            

            

            
        }


        private async void OnSyncButtonClicked(object sender, EventArgs e)
        {
            await _tableManager.SyncFilesAsync();
            BindListViewContents();
        }

        private async void OnLogRecordButtonClicked()
        {
            List<ConfirmRecord> allConfirmRecords = await _tableManager.GetAllLocalConfirmRecords();

            foreach (ConfirmRecord cR in allConfirmRecords)
            {
                Debug.WriteLine("Store Name : " + cR.StoreInformation +
                    " & Active Campaign : " + cR.ActiveCampaign +
                    " & Brand 1 : " + cR.Brand1 +
                    " & Brand 2 : " + cR.Brand2 +
                    " & Brand 3 : " + cR.Brand3 +
                    " & Notes : " + cR.Note +
                    " & Latitude : " + cR.Latitude +
                    " & Longitude : " + cR.Longitude +
                    " & Image Name : " + cR.ImageName +
                    " & IsUploaded : " + cR.IsUploaded +
                    " & Create At : " + cR.CreatedDate);
            }
        }

       



        public async void BindAllCampaigns()
        {
            try
            {
                campaignsList = await _tableManager.GetActiveCampaign();
                Name_Label_CampaignName.Text = campaignsList[0].CampaignName;
                brandsList = await _tableManager.GetBrands(campaignsList[0]);
                List<ConfirmRecord> confirmRecordList = await _tableManager.GetAllLocalConfirmRecords();
                Debug.WriteLine(confirmRecordList.Count);
                Debug.WriteLine(brandsList.Count);
                Debug.WriteLine(campaignsList.Count);
                //Name_PickerCampaign.Items.Clear();
                //if (campaignsList != null)
                //{
                //    foreach (Campaign campaign in campaignsList)
                //    {
                //        Name_PickerCampaign.Items.Add(campaign.CampaignName);
                //    }
                //}

                selectedCampaignID = campaignsList[0].Id;

                BindAllBrands(campaignsList[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine("No Campaigns");
            }

        }

        public async void BindAllBrands(Campaign campaign)
        {
            brandsList = await _tableManager.GetAllLocaBrands();
            Name_LabelBrand1.Text = brandsList[0].BrandName;
            Name_LabelBrand2.Text = brandsList[1].BrandName;
            Name_LabelBrand3.Text = brandsList[2].BrandName;
        }

        private string ReturnBrandID(int brandNumber, bool isBrandSelected)
        {
            if (isBrandSelected)
            {
                return brandsList[brandNumber].Id;
            }
            else
                return "";
        }

        private void ResetUI()
        {
            //Name_Label_NoImages.IsVisible = true;
            Name_ImagePlaceHolder.IsVisible = true;
            previewImage.Source = null;
            previewImage.IsVisible = false;

            //BindAllCampaigns();

            Name_SwitchBrand1.IsToggled = false;
            Name_SwitchBrand2.IsToggled = false;
            Name_SwitchBrand3.IsToggled = false;

            Name_EntryStoreInfo.Text = "";
            Name_EntryNote.Text = "";
        }

        private void DisplayAlert(List<bool> validationResult)
        {
            if (!validationResult[0])
            {
                Name_LabelCaptureImage.TextColor = Color.FromHex("FF0000");
            }
            else
            {
                Name_LabelCaptureImage.TextColor = Color.FromHex("FFFFFF");
            }
            if (!validationResult[1])
            {
                Name_LabelBrand.TextColor = Color.FromHex("FF0000");
                Name_ImageAlertBrands.IsVisible = true;
            }
            else
            {
                Name_LabelBrand.TextColor = Color.FromHex("FFFFFF");
                Name_ImageAlertBrands.IsVisible = false;
            }
            if (!validationResult[2])
            {
                Name_LabelStoreInfo.TextColor = Color.FromHex("FF0000");
                Name_ImageAlertStoreInfo.IsVisible = true;
                Name_LabelStoreInfo.IsVisible = true;
            }
            else
            {
                Name_LabelStoreInfo.TextColor = Color.FromHex("FFFFFF");
                Name_ImageAlertStoreInfo.IsVisible = false;
                Name_LabelStoreInfo.IsVisible = false;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IGeolocator locator = CrossGeolocator.Current;
            if (locator.IsGeolocationEnabled)
            {
                Debug.WriteLine("GPS Enabled");
                Name_ToolbarItemGPSStatus.Icon = "gpsconnected.png";
                Name_ToolbarItemGPSStatus.Text = "GPS ON";
            }
            else
            {
                Name_ToolbarItemGPSStatus.Text = "GPS OFF";
                Name_ToolbarItemGPSStatus.Icon = "gpsdisconnected.png";
            }
        }

        private void OnSwitchToggledBrand1(object sender, ToggledEventArgs e)
        {
            isBrand1Selected = e.Value;
        }

        private void OnSwitchToggledBrand2(object sender, ToggledEventArgs e)
        {
            isBrand2Selected = e.Value;
        }

        private void OnSwitchToggledBrand3(object sender, ToggledEventArgs e)
        {
            isBrand3Selected = e.Value;
        }
    }
}
