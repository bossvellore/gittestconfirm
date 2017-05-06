using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Confirm.Model
{
    public class Campaign
    {

        string id;
        string campaignName;
        bool isActive;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "campaignName")]
        public string CampaignName
        {
            get { return campaignName; }
            set { campaignName = value; }
        }

        [JsonProperty(PropertyName = "isactive")]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
    }
}
