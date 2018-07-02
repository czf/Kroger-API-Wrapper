using Czf.ApiWrapper.Kroger.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Responses
{
    public class SearchAllResponse
    {
        public List<string> Upcs { get; set; }
        public ProductsInfo ProductsInfo { get; set; }
    }
}
