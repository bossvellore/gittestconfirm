using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Confirm.Model
{
    public class Brand
    {
        string id;
        string brandName;
        string campaign;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "brandName")]
        public string BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        [JsonProperty(PropertyName = "campaign")]
        public string Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }
    }
}
