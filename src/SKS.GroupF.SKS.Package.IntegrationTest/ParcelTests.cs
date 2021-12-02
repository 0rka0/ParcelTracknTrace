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
            url = TestContext.Parameters.Get("url", "https://localhost:5001");

            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }


        [Test, Description("Submitting a parcel")]
        public async Task SubmitParcelAndTrackParcel()
        {
            Console.WriteLine($"Calling {url}/parcel");

            StreamReader reader = new StreamReader("testparcel.json");
            string fileContent = reader.ReadToEnd();
            var content = new StringContent(fileContent, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("/parcel", content);

            var jsonBody = await res.Content.ReadAsStringAsync();
            trackingId = JsonConvert.DeserializeObject<TrackingID>(jsonBody);
            
            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody, "No body returned");

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}");

            var resMsg = await client.GetAsync($"/parcel/{trackingId.trackingId}");
            var jsonBody2 = await resMsg.Content.ReadAsStringAsync();

            var trackingInfo = JsonConvert.DeserializeObject<TrackingInformation>(jsonBody2);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, resMsg.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", resMsg.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");
            Assert.AreEqual(TrackingInformation.StateEnum.PickupEnum, trackingInfo.State); ;
        }

        [Test, Description("ParcelJourney")]
        public async Task SubmitParcelTrackJourney()
        {
            Console.WriteLine($"Calling {url}/parcel");

            StreamReader reader = new StreamReader("testparcel.json");
            string fileContent = reader.ReadToEnd();
            var content = new StringContent(fileContent, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("/parcel", content);

            var jsonBody = await res.Content.ReadAsStringAsync();
            trackingId = JsonConvert.DeserializeObject<TrackingID>(jsonBody);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody, "No body returned");

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}/reportHop/code - x3");

            var res2 = await client.PostAsync($"/parcel/{trackingId.trackingId}/reportHop/WTTA080", null);
            res2 = await client.PostAsync($"/parcel/{trackingId.trackingId}/reportHop/WENB01", null);
            res2 = await client.PostAsync($"/parcel/{trackingId.trackingId}/reportHop/WTTA046", null);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res2.StatusCode, "Status Code not 200-OK");

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}");

            var resMsg = await client.GetAsync($"/parcel/{trackingId.trackingId}");
            var jsonBody2 = await resMsg.Content.ReadAsStringAsync();

            var trackingInfo = JsonConvert.DeserializeObject<TrackingInformation>(jsonBody2);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, resMsg.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", resMsg.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");
            Assert.AreEqual(TrackingInformation.StateEnum.InTruckDeliveryEnum, trackingInfo.State);
            Assert.AreEqual(3, trackingInfo.VisitedHops.Count);
        }

        [Test, Description("ParcelDelivery")]
        public async Task SubmitParcelTrackDelivery()
        {
            Console.WriteLine($"Calling {url}/parcel");

            StreamReader reader = new StreamReader("testparcel.json");
            string fileContent = reader.ReadToEnd();
            var content = new StringContent(fileContent, Encoding.UTF8, "application/json");

            var res = await client.PostAsync("/parcel", content);

            var jsonBody = await res.Content.ReadAsStringAsync();
            trackingId = JsonConvert.DeserializeObject<TrackingID>(jsonBody);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody, "No body returned");

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}/reportDelivery");

            var res2 = await client.PostAsync($"/parcel/{trackingId.trackingId}/reportDelivery", null);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res2.StatusCode, "Status Code not 200-OK");

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}");

            var resMsg = await client.GetAsync($"/parcel/{trackingId.trackingId}");
            var jsonBody2 = await resMsg.Content.ReadAsStringAsync();

            var trackingInfo = JsonConvert.DeserializeObject<TrackingInformation>(jsonBody2);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, resMsg.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", resMsg.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");
            Assert.AreEqual(TrackingInformation.StateEnum.DeliveredEnum, trackingInfo.State);
        }

        [Test, Description("TransferringParcel")]
        public async Task TransferParcel()
        {
            var trackingId = "QZJRB4HZ7";
            Console.WriteLine($"Calling {url}/parcel/{trackingId}");

            StreamReader reader = new StreamReader("testparcel.json");
            string fileContent = reader.ReadToEnd();
            var content = new StringContent(fileContent, Encoding.UTF8, "application/json");

            var res = await client.PostAsync($"/parcel/{trackingId}", content);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");

            var resMsg = await client.GetAsync($"/parcel/{trackingId}");
            var jsonBody2 = await resMsg.Content.ReadAsStringAsync();

            var trackingInfo = JsonConvert.DeserializeObject<TrackingInformation>(jsonBody2);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, resMsg.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", resMsg.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");
            Assert.AreEqual(TrackingInformation.StateEnum.PickupEnum, trackingInfo.State);
        }
    }
}
