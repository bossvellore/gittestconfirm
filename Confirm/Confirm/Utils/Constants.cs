using System;
using System.Collections.Generic;
using System.Text;

namespace Confirm.Utils
{
    public static class Constants
    {
        public static string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=confirmblob;AccountKey=2UWG6GkSTWD8ufR8TJH0oOHrHDdFECsNukak+L4dwcKt+OA3TbPah7dHjvEH8oLpIQMkU3aDMtj4c2C3AUFdkQ==;EndpointSuffix=core.windows.net";
        public static string ApplicationURL = "http://confirmapp.azurewebsites.net";
        public static string LocalDBName = "localconfirmdatabase.db";
        public static string ContainerName = "ImageContainer";
        internal static string ImageStorgePath = "/Android/data/Confirm.Android/files/Pictures/ConfirmAPP/";
    }
}
