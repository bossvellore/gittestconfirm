using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Confirm.Model
{
    public class ConfirmRecord
    {
        string id;
        string activeCampaign;
        string brand1;
        string brand2;
        string brand3;
        string storeInformation;
        string note;
        double latitude;
        double longitude;
        string imageName;
        bool isUploaded = false;
        string createdDate;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "activeCampaign")]
        public string ActiveCampaign
        {
            get { return activeCampaign; }
            set { activeCampaign = value; }
        }

        [JsonProperty(PropertyName = "brand1")]
        public string Brand1
        {
            get { return brand1; }
            set { brand1 = value; }
        }

        [JsonProperty(PropertyName = "brand2")]
        public string Brand2
        {
            get { return brand2; }
            set { brand2 = value; }
        }

        [JsonProperty(PropertyName = "brand3")]
        public string Brand3
        {
            get { return brand3; }
            set { brand3 = value; }
        }

        [JsonProperty(PropertyName = "storeInformation")]
        public string StoreInformation
        {
            get { return storeInformation; }
            set { storeInformation = value; }
        }

        [JsonProperty(PropertyName = "note")]
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        [JsonProperty(PropertyName = "imageName")]
        public string ImageName
        {
            get { return imageName; }
            set { imageName = value; }
        }

        [JsonProperty(PropertyName = "isUploaded")]
        public bool IsUploaded
        {
            get { return isUploaded; }
            set { isUploaded = value; }
        }

        [JsonProperty(PropertyName = "createdDate")]
        public string CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }
    }
}
