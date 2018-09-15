using Czf.ApiWrapper.Kroger.Requests;
using Czf.ApiWrapper.Kroger.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using RestSharp.Deserializers;
using RestSharp.Serializers;
using RestSharp;
using System.IO;

namespace Czf.ApiWrapper.Kroger
{

    /// <summary>
    /// Tls 1.2 should be added to acceptable security protocols before instantiation of any httpclient objects. 
    /// <code>ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;</code>
    /// </summary>
    public class KrogerClient : IDisposable
    {
        #region private
        private static readonly Uri BASE_URL = new Uri("https://www.kroger.com/");
        private const string STORES_GRAPHQL_ENDPOINT = "stores/api/graphql";
        ///two parameters searchtext={0}, filters={1}
        private const string STORE_SEARCH_PAYLOAD = "{{\"query\":\"\\n  query storeSearch($searchText: String!, $filters: [String]!) {{\\n        storeSearch(searchText: $searchText, filters: $filters) {{\\n          stores {{\\n            ...storeSearchResult\\n          }}\\n          fuel {{\\n            ...storeSearchResult\\n          }}\\n          shouldShowFuelMessage\\n        }}\\n      }}\\n      \\n  fragment storeSearchResult on Store {{\\n    banner\\n    vanityName\\n    divisionNumber\\n    storeNumber\\n    phoneNumber\\n    showWeeklyAd\\n    showShopThisStoreAndPreferredStoreButtons\\n    distance\\n    latitude\\n    longitude\\n    address {{\\n      addressLine1\\n      addressLine2\\n      city\\n      countryCode\\n      stateCode\\n      zip\\n    }}\\n    pharmacy {{\\n      phoneNumber\\n    }}\\n    departments {{\\n      code\\n    }}\\n    fulfillmentMethods{{\\n      hasPickup\\n      hasDelivery\\n                 }}\\n  }}\\n\",\"variables\":{{\"searchText\":\"{0}\",\"filters\":[{1}]}},\"operationName\":\"storeSearch\"}}";

        ///four parameters start={0}, count={1}, query={2}, tab={3}
        private const string SITE_SEARCH_ALL= "search/api/searchAll?start={0}&count={1}&query={2}&tab={3}&monet=true";

        private const string PRODUCTS_DETAILS_ENDPOINT = "products/api/products/details";
        private bool disposedValue = false;
        private HttpClient _client;

        #endregion

        #region Constructors
        /// <summary>
        /// Tls 1.2 should be added to acceptable security protocols before instantiation of any httpclient objects. 
        /// <code>ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;</code>
        /// </summary>
        public KrogerClient() : this(new HttpClient(new HttpClientHandler() { UseCookies = false })) { }

        /// <summary>
        /// Create a client.
        /// Tls 1.2 should be added to acceptable security protocols before instantiation of any httpclient objects. 
        /// <code>ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;</code>
        /// </summary>
        /// <param name="client">client should have a client handler that has UseCookies: false</param>
        public KrogerClient(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("*/*"));
            _client.DefaultRequestHeaders.Add("Cookie", "");
            _client.DefaultRequestHeaders.Add("User-Agent", "_/_");
            //_client.DefaultRequestHeaders.Add("accept-encoding","gzip, deflate");
            _client.BaseAddress = BASE_URL;
            _client.Timeout.Add(new TimeSpan(0, 3, 0));
            
            //_client.DefaultRequestHeaders.ConnectionClose = true;
        }
        #endregion Constructors


        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
         
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

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
                using (HttpContent content = new StringContent(payload))
                {
                    content.Headers.ContentType = MediaTypeWithQualityHeaderValue.Parse("application/json");
                    using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, PRODUCTS_DETAILS_ENDPOINT) { Content = content })
                    {
                        requestMessage.Headers.ExpectContinue = false;
                        
                        if (request.StoreId != null && request.DivisionId != null)
                        {
                            requestMessage.Headers.Add("store-id", request.StoreId);
                            requestMessage.Headers.Add("division-id", request.DivisionId);
                        }

                        using (HttpResponseMessage response = await _client.SendAsync(requestMessage))
                        {
                            try
                            {
                                string responseContent = response.Content.ReadAsStringAsync().Result;
                                productDetailsResponse = JsonConvert.DeserializeObject<ProductsDetailsResponse>(responseContent);
                            }
                            catch (JsonReaderException)
                            {
                                failed = true;
                            }
                        }
                    }
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
            using (HttpResponseMessage responseMsg =  await _client.PostAsync(urlQuery, new StringContent("")))
            {

                string responseContent = responseMsg.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<SearchAllResponse>(responseContent);
            }

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
                using (HttpContent content = new StringContent(payload))
                using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, STORES_GRAPHQL_ENDPOINT) { Content =content})
                using (HttpResponseMessage response = await _client.SendAsync(requestMessage))
                {
                    try
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        storeSearchResponse = JsonConvert.DeserializeObject<StoreSearchResponse>(responseContent, new StoreSearchResponseCustomJsonConverter());
                    }
                    catch (JsonReaderException)
                    {
                        failed = true;
                    }            
                }
            }while (tries > 0 && failed);
            return storeSearchResponse;
        }

        /// <summary>
        /// Synchronoously fetch stores using the specified input
        /// </summary>
        /// <param name="zipCity"></param>
        /// <returns></returns>
        public StoreSearchResponse StoreSearch(string zipCity)
        {
            return StoreSearchAsync(new StoreSearchRequest(zipCity)).Result;
        }

        /// <summary>
        /// Synchronoously fetch stores using the request
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
            request.JsonSerializer = new RestSharpJsonSerializer();
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




public class RestSharpJsonSerializer : ISerializer, IDeserializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        public RestSharpJsonSerializer()
        {
            ContentType = "application/json";
            _serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            };
        }

        public RestSharpJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            ContentType = "application/json";
            _serializer = serializer;
        }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    _serializer.Serialize(jsonTextWriter, obj);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

        public string DateFormat { get; set; }
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string ContentType { get; set; }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
