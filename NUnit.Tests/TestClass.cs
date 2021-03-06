﻿using NUnit.Framework;
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            KrogerClient client = new KrogerClient();
            ProductsDetailsResponse response = client.ProductsDetailsAsync(
                new ProductsDetailsRequest()
                {
                    UPCs = new List<string>() { "0001111087720","0001111060828","0001111060826"},
                    FilterBadProducts = false
                }
                ).Result;

            response = client.ProductsDetailsAsync(
                new ProductsDetailsRequest()
                {
                    UPCs = new List<string>() { "0001111087720", "0001111060828", "0001111060826" },
                    FilterBadProducts = false
                }
                ).Result;
        }

        [Test]
        public void AProductsDetailsWithStoreInfoTest()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

            response = client.ProductsDetailsAsync(
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
            System.Net.ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls12;
            KrogerClient client = new KrogerClient();
            var response = client.StoreSearchAsync("kansas city").Result;
            var response2 = client.StoreSearchAsync("kansas city").Result;

            Assert.NotNull(response);
            Assert.NotNull(response2);
            Assert.NotNull(response.Fuel);
            Assert.NotNull(response2.Fuel);
            Assert.NotNull(response.Stores);
            Assert.NotNull(response2.Stores);
            
            Assert.That(response.Fuel.Any(x => x.DivisionNumber != null));
            Assert.That(response2.Fuel.Any(x => x.DivisionNumber != null));
            Assert.That(response.Stores.Any(x => x.StoreNumber != null));
            Assert.That(response2.Stores.Any(x => x.StoreNumber != null));

        }


        
        public void RestSharpTest()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            KrogerClient client = new KrogerClient();
            var r = client.ProductRestSharp(new ProductsDetailsRequest()
            {
                UPCs = new List<string>() { "0001111087720", "0001111060828", "0001111060826" },
                FilterBadProducts = false
            });


            Assert.NotNull(r.Products);
            Assert.That(r.Products.Any());

        }
    }
}
