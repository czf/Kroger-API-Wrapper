using Czf.ApiWrapper.Kroger.Requests;
using Czf.ApiWrapper.Kroger.Responses;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using RestSharp.Deserializers;

using RestSharp;
using Czf.ApiWrapper.Kroger.Restsharp;

namespace Czf.ApiWrapper.Kroger
{

    /// <summary>
    /// Tls 1.2 should be added to acceptable security protocols before instantiation of any httpclient objects. 
    /// <code>ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;</code>
    /// </summary>
    public class KrogerClient
    {
        #region private
        private static readonly Uri BASE_URL = new Uri("https://www.kroger.com/");
        private const string STORES_GRAPHQL_ENDPOINT = "stores/api/graphql";
        ///two parameters searchtext={0}, filters={1}
        private const string STORE_SEARCH_PAYLOAD = "{{\"query\":\"\\n      query storeSearch($searchText: String!, $filters: [String]!) {{\\n        storeSearch(searchText: $searchText, filters: $filters) {{\\n          stores {{\\n            ...storeSearchResult\\n          }}\\n          fuel {{\\n            ...storeSearchResult\\n          }}\\n          shouldShowFuelMessage\\n        }}\\n      }}\\n      \\n  fragment storeSearchResult on Store {{\\n    banner\\n    vanityName\\n    divisionNumber\\n    storeNumber\\n    phoneNumber\\n    showWeeklyAd\\n    showShopThisStoreAndPreferredStoreButtons\\n    distance\\n    latitude\\n    longitude\\n    address {{\\n      addressLine1\\n      addressLine2\\n      city\\n      countryCode\\n      stateCode\\n      zip\\n    }}\\n    pharmacy {{\\n      phoneNumber\\n    }}\\n    departments {{\\n      code\\n    }}\\n    fulfillmentMethods{{\\n      hasPickup\\n      hasDelivery\\n    }}\\n  }}\\n\",\"variables\":{{\"searchText\":\"{0}\",\"filters\":[{1}]}},\"operationName\":\"storeSearch\"}}";

        ///four parameters start={0}, count={1}, query={2}, tab={3}
        private const string SITE_SEARCH_ALL= "search/api/searchAll?start={0}&count={1}&query={2}&tab={3}&monet=true";

        private const string PRODUCTS_DETAILS_ENDPOINT = "products/api/products/details";
        private bool disposedValue = false;
        private IRestClient _client;

        #endregion

        #region Constructors
        /// <summary>
        /// Tls 1.2 should be added to acceptable security protocols before instantiation of any httpclient objects. 
        /// <code>ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;</code>
        /// </summary>
        public KrogerClient() : this(new RestClient()) { }

        ///// <summary>
        ///// Create a client.
        ///// Tls 1.2 should be added to acceptable security protocols before instantiation of any httpclient objects. 
        ///// <code>ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;</code>
        ///// </summary>
        ///// <param name="client"></param>
        public KrogerClient(IRestClient client)
        {
            _client = client;
            _client.ClearHandlers();

            IDeserializer deserializer = new NewtonsoftJsonSerializer ();

            _client.AddHandler(MediaTypeWithQualityHeaderValue.Parse("*/*").MediaType, deserializer);
            _client.AddHandler(MediaTypeWithQualityHeaderValue.Parse("application/json;charset=UFT-8").MediaType, deserializer);
            _client.AddDefaultHeader("Cookie", "_=_;");
            _client.BaseUrl = BASE_URL;
            _client.UserAgent = "_/_";
        }
        #endregion Constructors


        #region Public
        /// <summary>
        /// Get detailed information for specified products
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ProductsDetailsResponse> ProductsDetailsAsync(ProductsDetailsRequest request)
        {
            ProductsDetailsResponse productDetailsResponse = null;
            string payload = JsonConvert.SerializeObject(request);
            int tries = 3;
            bool failed = false;
            do
            {
                tries--;
                failed = false;
                try
                {
                    IRestRequest restRequest = new RestRequest(PRODUCTS_DETAILS_ENDPOINT, Method.POST);
                    restRequest.JsonSerializer = new NewtonsoftJsonSerializer ();
                    restRequest.AddJsonBody(request);
                    restRequest.AddCookie("_", "_"); //a cookie is required by the request contract, 
                                                     //restsharp requires that this not be an empty string in order for the header be set
                    if (request.StoreId != null && request.DivisionId != null)
                    {
                        restRequest.AddHeader("store-id", request.StoreId);
                        restRequest.AddHeader("division-id", request.DivisionId);
                    }


                    IRestResponse<ProductsDetailsResponse> restResponse = await _client.ExecutePostTaskAsync<ProductsDetailsResponse>(restRequest);
                    productDetailsResponse = restResponse.Data;
                                
                }
                catch (JsonReaderException)
                {
                    failed = true;
                }
            } while (tries > 0 && failed);
            return productDetailsResponse;
        }

        /// <summary>
        /// Get detailed information for specified products
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ProductsDetailsResponse ProductsDetails(ProductsDetailsRequest request)
        {
            return ProductsDetailsAsync(request).Result;
        }

        /// <summary>
        /// Peform a keyword search across site content
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SearchAllResponse> SearchAllAsync(SearchAllRequest request)
        {
            SearchAllResponse response = null;
            string urlQuery = string.Format(SITE_SEARCH_ALL, request.Start, request.Count, request.Query, 0); //0 products tab

            

            IRestRequest restRequest = new RestRequest(urlQuery, Method.POST);
            restRequest.AddCookie("_", "_"); //a cookie is required by the request contract, 
                                             //restsharp requires that this not be an empty string in order for the header be set

            IRestResponse<SearchAllResponse> restResponse = await _client.ExecutePostTaskAsync<SearchAllResponse>(restRequest);
            response = restResponse.Data;
            

            return response;
        }

        /// <summary>
        /// Peform a keyword search across site content
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SearchAllResponse SearchAll(SearchAllRequest request)
        {
            return SearchAllAsync(request).Result;
        }
        /// <summary>
        /// Return stores near the specificed input
        /// </summary>
        /// <param name="zipCity">US zipcode or city/state</param>
        /// <returns></returns>
        public async Task<StoreSearchResponse> StoreSearchAsync(string zipCity)
        {
            return await StoreSearchAsync(new StoreSearchRequest(zipCity));
        }

        /// <summary>
        /// Return stores using the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<StoreSearchResponse> StoreSearchAsync(StoreSearchRequest request)
        {
            StoreSearchResponse storeSearchResponse = null;
            string payload = string.Format(STORE_SEARCH_PAYLOAD, request.SearchText, string.Empty);
            int tries = 3;
            bool failed = false;
            do
            {
                tries--;
                failed = false;
                
                try
                {
                    IRestRequest restRequest = new RestRequest(STORES_GRAPHQL_ENDPOINT, Method.POST);
                    restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
                    restRequest.AddCookie("_", "_"); //a cookie is required by the request contract, 
                                                     //restsharp requires that this not be an empty string in order for the header be set
                                        
                    IRestResponse<StoreSearchResponse> restResponse = 
                        await _client.ExecutePostTaskAsync<StoreSearchResponse>(restRequest);
                    storeSearchResponse = restResponse.Data;
                }
                catch (JsonReaderException)
                {
                    failed = true;
                }
                
            }while (tries > 0 && failed);
            return storeSearchResponse;
        }

        /// <summary>
        /// Synchronously fetch stores using the specified input
        /// </summary>
        /// <param name="zipCity"></param>
        /// <returns></returns>
        public StoreSearchResponse StoreSearch(string zipCity)
        {
            return StoreSearchAsync(new StoreSearchRequest(zipCity)).Result;
        }

        /// <summary>
        /// Synchronously fetch stores using the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public StoreSearchResponse StoreSearch(StoreSearchRequest request)
        {
            return StoreSearchAsync(request).Result;
        }

        
               
        public ProductsDetailsResponse ProductRestSharp(ProductsDetailsRequest prodRequest)
        {
            var client = new RestClient("https://www.kroger.com/");
            
            client.UserAgent = "_/_";
            client.AutomaticDecompression = false;
            var request = new RestRequest(PRODUCTS_DETAILS_ENDPOINT, Method.POST);
            request.AddHeader("Accept-Encoding", null);
            request.AddHeader("User-Agent", "_/_");
            request.AddCookie("X", "5");
            request.AddHeader("Accept", "*/*");
            request.JsonSerializer = new NewtonsoftJsonSerializer ();
            request.AddJsonBody(prodRequest);
            
            
            IRestResponse<ProductsDetailsResponse> response = client.Execute<ProductsDetailsResponse>(request);
            var f = response.Data;
             response = client.Execute<ProductsDetailsResponse>(request);
            return response.Data;
            
            //_client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("*/*"));
            //_client.DefaultRequestHeaders.Add("Cookie", "");
            //_client.DefaultRequestHeaders.Add("User-Agent", "_/_");

        }

        #endregion Public
    }




    
}
