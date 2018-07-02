using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Domain
{
    public class ProductsInfo
    {
        public string SearchId { get; set; }
        public int TotalCount { get; set; }
        public bool hasError { get; set; }


        //TODO: there are more properties to do.
        #region TODOs
        /* brands
         * coupons
         * departments
         * fulfillment
         * nutrition
        */
        #endregion TODOs
    }
}
