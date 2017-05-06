using Confirm.Model;
using Confirm.Utils;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confirm.AzureCloudManager
{
    public static class AzureStorageManager
    {
        public static CloudBlobContainer GetContainer(string containerType)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(Constants.StorageConnectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerType.ToString().ToLower());
            //container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            container.CreateIfNotExistsAsync();
            return container;
        }

        public static async Task<IList<string>> GetFilesListAsync(string containerType)
        {
            var container = GetContainer(containerType);

            var allBlobsList = new List<string>();
            BlobContinuationToken token = null;

            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    allBlobsList.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);

            return allBlobsList;
        }

        public static async Task<byte[]> GetFileAsync(string containerType, string name)
        {
            var container = GetContainer(containerType);

            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }

        public static async Task<string> UploadFileAsync(string containerType, Stream stream, string name)
        {
            var container = GetContainer(containerType);
            //await container.CreateIfNotExistsAsync();
            //var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            //fileBlob.UploadFromFileAsync()
            //StorageFile.GetFileFromPathAsync("");
            await fileBlob.UploadFromStreamAsync(stream);
            Debug.WriteLine("UploadFileAsync Name : " + name);
            return name;
        }
        public static async Task<string> UploadFileAsync(string containerType, byte[] bytes, string name)
        {
            var container = GetContainer(containerType);
            //await container.CreateIfNotExistsAsync();
            //var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            //fileBlob.UploadFromFileAsync()
            //StorageFile.GetFileFromPathAsync("");
            await fileBlob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
            return name;
        }

        public static async Task<bool> DeleteFileAsync(string containerType, string name)
        {
            var container = GetContainer(containerType);
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }

        public static async Task<bool> DeleteContainerAsync(string containerType)
        {
            var container = GetContainer(containerType);
            return await container.DeleteIfExistsAsync();
        }
    }
}
