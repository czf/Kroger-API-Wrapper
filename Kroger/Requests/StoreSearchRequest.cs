using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Requests
{
    /// <summary>
    /// Search for Kroger stores using search text
    /// </summary>
    public class StoreSearchRequest
    {

        /// <summary>
        /// zip code or city/state
        /// </summary>
        public string SearchText { get; protected set; }

        public StoreSearchRequest(string searchText)
        {
            SearchText = searchText;
        }

    }
}
