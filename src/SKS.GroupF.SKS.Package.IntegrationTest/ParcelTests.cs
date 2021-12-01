using Newtonsoft.Json;
using NUnit.Framework;
using SKSGroupF.SKS.Package.Services.DTOs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SKS.GroupF.SKS.Package.IntegrationTest
{
    class TrackingID
    {
        public string trackingId { get; set; }
    }
    public class ParcelTests
    {
        private string url;
        private HttpClient client;
        private TrackingID trackingId;

        [SetUp]
        public void Setup()
        {
            url = TestContext.Parameters.Get("url", "https://sksgroupf-webapp.azurewebsites.net");

            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }


        [Test, Description("Submitting a parcel")]
        public async Task SubmitParcel()
        {
            Console.WriteLine($"Calling {url}/parcel");

            StreamReader reader = new StreamReader("../../../testparcel.json");
            string fileContent = reader.ReadToEnd();
            var content = new StringContent(fileContent, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("/parcel", content);

            var jsonBody = await res.Content.ReadAsStringAsync();
            trackingId = JsonConvert.DeserializeObject<TrackingID>(jsonBody);
            
            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody, "No body returned");


            var resMsg = await client.GetAsync($"/parcel/{trackingId.trackingId}");
            var jsonBody2 = await resMsg.Content.ReadAsStringAsync();

            var trackingInfo = JsonConvert.DeserializeObject<TrackingInformation>(jsonBody2);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, resMsg.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", resMsg.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");
            Assert.AreEqual(TrackingInformation.StateEnum.PickupEnum, trackingInfo.State); ;
        }

    }

    
}
