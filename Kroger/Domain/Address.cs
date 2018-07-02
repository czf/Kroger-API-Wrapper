using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Domain
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string Zip { get; set; }
    }
}
