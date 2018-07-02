using Czf.ApiWrapper.Kroger.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Czf.ApiWrapper.Kroger.Responses
{
    public class StoreSearchResponse
    { 

        public List<Store> Stores { get; set; }
        public List<Store> Fuel { get; set; }
        public bool ShouldShowFuelMessage { get; set; }
    }




    

    

   
    
}