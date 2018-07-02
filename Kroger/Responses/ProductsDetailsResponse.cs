using Czf.ApiWrapper.Kroger.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Requests
{
    public class ProductsDetailsResponse
    {
        public List<ProductDetail> Products { get; set; }
        //TODO Coupon
        //TODO departments
        public bool PriceHasError { get; set; }
        public int TotalCount { get; set; }
    }
}
