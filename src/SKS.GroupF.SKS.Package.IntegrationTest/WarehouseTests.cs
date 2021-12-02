using Newtonsoft.Json;
using NUnit.Framework;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SKS.GroupF.SKS.Package.IntegrationTest
{
    public class WarehouseTests
    {
        private string url;
        private HttpClient client;

        [SetUp]
        public void Setup()
        {
            url = TestContext.Parameters.Get("url", "https://localhost:5001");

            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }

        [Order(1), Test, Description("Importing Warehouse Tree")]
        public async Task ImportWarehouse()
        {
            Console.WriteLine($"Calling {url}/Warehouse");
            StreamReader reader = new StreamReader("trucks-new2-light-transferwh.json");
            string fileContent = reader.ReadToEnd();
            var content = new StringContent(fileContent, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("/Warehouse", content);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");

            var resMsg = await client.GetAsync("/Warehouse");
            var jsonBody = await resMsg.Content.ReadAsStringAsync();

            Assert.That(jsonBody, Contains.Substring("Warehouse Level 1 - Wien"), "Sample Text not within the JSON");
        }
         
        [Order(2), Test, Description("Exporting Warehouse Tree")]
        public async Task ExportWarehouse()
        {
            Console.WriteLine($"Calling {url}/Warehouse");
            var res = await client.GetAsync("/Warehouse");
            var jsonBody = await res.Content.ReadAsStringAsync();

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody, "No body returned");
            Assert.That(jsonBody, Contains.Substring("Warehouse Level 1 - Wien"), "Sample Text not within the JSON");
        }


        [Order(3), Test, Description("Getting Warehouse Tree")]
        public async Task GetSingleWarehouseByCode()
        {
            var code = "WTTA080";
            Console.WriteLine($"Calling {url}/Warehouse/{code}");

            var res = await client.GetAsync($"/Warehouse/{code}");
            var jsonBody = await res.Content.ReadAsStringAsync();
            var truck = JsonConvert.DeserializeObject<Truck>(jsonBody);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody, "No body returned");
            Assert.AreEqual(code, truck.Code);
        }


    }
}