using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Domain
{
    public class Store
    {
        public string Banner { get; set; }
        public string VanityName { get; set; }
        public string DivisionNumber { get; set; }
        public string StoreNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool ShowWeeklyAd { get; set; }
        public bool ShowShopThisStoreAndPreferredStoreButtons { get; set; }
        public decimal Distance { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Address Address { get; set; }
        public object Pharmacy { get; set; } //unknown type
        public List<Department> Departments { get; set; }
        public FulfillmentMethods FulfillmentMethods { get; set; }
    }
}
