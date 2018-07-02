using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Czf.ApiWrapper.Kroger;
using System.Threading;
using System.Net;
using RestSharp;
using System.Net.Http;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using Czf.ApiWrapper.Kroger.Requests;

namespace NUnit.Tests
{
    /// <summary>
    /// All of these tests connect to the live api.
    /// </summary>
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void ProductsDetailsTest()
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            KrogerClient client = new KrogerClient();
            ProductsDetailsResponse response = client.ProductsDetailsAsync(
                new ProductsDetailsRequest()
                {
                    UPCs = new List<string>() { "0001111087720","0001111060828","0001111060826"},
                    FilterBadProducts = false
                }
                ).Result;
        }

        [Test]
        public void ProductsDetailsWithStoreInfoTest()
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            KrogerClient client = new KrogerClient();
            ProductsDetailsResponse response = client.ProductsDetailsAsync(
                new ProductsDetailsRequest()
                {
                    UPCs = new List<string>() { "0001111087720", "0001111060828", "0001111060826" },
                    FilterBadProducts = false,
                    StoreId = "00122",
                    DivisionId = "701"
                }
                ).Result;
            Assert.NotNull(response.Products);
            Assert.That(response.Products.Any());
        }


        [Test]
        public void SeachAllTest()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            KrogerClient client = new KrogerClient();
            var response = client.SearchAllAsync(new SearchAllRequest() { Query = "bananas" }).Result;
            Assert.NotNull(response);
            Assert.NotNull(response.Upcs);
            Assert.That(response.Upcs.Any(x => x.Length > 0));
        }

        [Test]
        public void StoreSearchTest()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            KrogerClient client = new KrogerClient();
            var response = client.StoreSearchAsync("kansas city").Result;

            Assert.NotNull(response);
            Assert.NotNull(response.Fuel);
            Assert.NotNull(response.Stores);
            
            Assert.That(response.Fuel.Any(x => x.DivisionNumber != null));
            Assert.That(response.Stores.Any(x => x.StoreNumber != null));

            
        }
    }
}
