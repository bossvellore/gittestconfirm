using Confirm.Dependency;
using Confirm.Model;
using Confirm.Utils;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Confirm.AzureCloudManager
{
    public class ConfirmTableManager
    {

        MobileServiceClient client;
        IMobileServiceSyncTable<ConfirmRecord> confirmTable;
        IMobileServiceSyncTable<Brand> brandTable;
        IMobileServiceSyncTable<Campaign> campaignTable;

        public ConfirmTableManager()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);
            
            
            var store = new MobileServiceSQLiteStore(Constants.LocalDBName);
            store.DefineTable<ConfirmRecord>();
            store.DefineTable<Brand>();
            store.DefineTable<Campaign>();
            client.SyncContext.InitializeAsync(store);
            
            confirmTable = client.GetSyncTable<ConfirmRecord>();
            brandTable = client.GetSyncTable<Brand>();
            campaignTable = client.GetSyncTable<Campaign>();

          


        }
        
        public async void putCampaign(Campaign campaign)
        {
            if (campaign.Id == null)
                
                await campaignTable.InsertAsync(campaign);
            else
                await campaignTable.UpdateAsync(campaign);

            await SyncLocalDBWithAzureDB();
        }
        
        public async Task SyncLocalDBWithAzureDB()
        {
            Debug.WriteLine("ConfrimTableManager : Sync Started");
            await client.SyncContext.PushAsync();
            await confirmTable.PullAsync("allConfirmRecord", confirmTable.CreateQuery());
            await brandTable.PullAsync("allBrands", brandTable.CreateQuery());
            await campaignTable.PullAsync("allCampaigns", campaignTable.CreateQuery());
        }
        public async Task<List<ConfirmRecord>> GetAllLocalConfirmRecords()
        {
            List<ConfirmRecord> allConfirmRecords = await confirmTable.ToListAsync();
            return allConfirmRecords;
        }
        public async Task<List<Brand>> GetAllLocaBrands()
        {
            List<Brand> brandList = await brandTable.ToListAsync();
            return brandList;
        }
        public async Task<List<Brand>> GetBrands(Campaign campaign)
        {
            List<Brand> brandList = await brandTable.Where(brand => brand.Campaign == campaign.Id).ToListAsync();
            return brandList;
        }
        public async Task<List<Campaign>> GetAllLocalCampaigns()
        {
            List<Campaign> allLocalCampaigns = await campaignTable.ToListAsync();
            return allLocalCampaigns;
        }

        public async Task<List<Campaign>> GetActiveCampaign()
        {
            List<Campaign> allLocalCampaigns = await campaignTable.Where(campaign => campaign.IsActive).ToListAsync();
            return allLocalCampaigns;
        }

        public async Task UpsertRecord(ConfirmRecord confirmRecord)
        {
            if (confirmRecord.Id == null)
                await confirmTable.InsertAsync(confirmRecord);
            else
                await confirmTable.UpdateAsync(confirmRecord);
        }

        private async Task<List<ConfirmRecord>> GetPendingImages()
        {
            return await confirmTable.Where(confirmRecord => !confirmRecord.IsUploaded).ToListAsync();
            //return await GetAllLocalConfirmRecords();
        }
        public async Task SyncFilesAsync()
        {
            try
            {
                //Uploading local images
                List<ConfirmRecord> imageUploadPendingrecords = await GetPendingImages();
                Debug.WriteLine("AzureStorageManager.SyncFilesAsync : " + imageUploadPendingrecords.Count);
                foreach (ConfirmRecord imageUploadPendingrecord in imageUploadPendingrecords)
                {
                    Debug.WriteLine("AzureStorageManager.SyncFilesAsync : " + "Starting Upload");
                    FileStream fileStream = null;
                    try
                    {
                        
                        var externalStorageDependecy = DependencyService.Get<IGetExternalStoragePath>();
                        string imagesFolderPath = externalStorageDependecy.GetExternalStoragePath()+ Constants.ImageStorgePath;
                       
                        fileStream = File.OpenRead(imagesFolderPath + imageUploadPendingrecord.ImageName);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                    Debug.WriteLine("Length of filestream : " + fileStream.Length);
                    await AzureStorageManager.UploadFileAsync(Constants.ContainerName, fileStream, imageUploadPendingrecord.ImageName)
                      .ContinueWith(async (T) =>
                      {

                          if (T.Status == TaskStatus.RanToCompletion)
                          {
                              imageUploadPendingrecord.IsUploaded = true;
                              await UpsertRecord(imageUploadPendingrecord);
                              Debug.WriteLine("Image Uploaded successfully : " + imageUploadPendingrecord.ImageName);
                          }
                          else
                              Debug.WriteLine("Image Uploaded failed : " + imageUploadPendingrecord.ImageName);
                          Debug.WriteLine("UploadResult : " + " IsCanceled : " + T.IsCanceled);
                          Debug.WriteLine("UploadResult : " + " IsCompleted : " + T.IsCompleted);
                          Debug.WriteLine("UploadResult : " + " IsFaulted : " + T.IsFaulted);
                          Debug.WriteLine("UploadResult : " + " Result : " + T.Result);
                          Debug.WriteLine("UploadResult : " + " Status : " + T.Status);
                      });
                }
                await SyncLocalDBWithAzureDB();

            }
            catch (Exception ex)
            {
                var a = ex;
                Debug.WriteLine("ConfirmTableManager : " + ex);
            }

        }


    }
}
