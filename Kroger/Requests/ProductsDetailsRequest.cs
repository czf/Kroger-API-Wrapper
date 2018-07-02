using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Requests
{
    public class ProductsDetailsRequest
    {
        [JsonProperty(PropertyName ="upcs")]
        public List<string> UPCs { get; set; }
        public bool FilterBadProducts { get; set; }

        [JsonIgnore]
        public string StoreId { get; set; }
        [JsonIgnore]
        public string DivisionId { get; set; }
    }
}
