using Microsoft.AspNetCore.Http;
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
    public class WebhookTests
    {
        private string url;
        private HttpClient client;
        TrackingID trackingId;
        private string testUrl = "https://test.com";

        [SetUp]
        public void Setup()
        {
            url = TestContext.Parameters.Get("url", "https://localhost:5001");

            client = new HttpClient()
            {
                BaseAddress = new Uri(url)
            };
        }

        [Order(8), Test, Description("Subscribing to parcel")]
        public async Task CreateWebhook()
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

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}/webhooks");

            var res2 = await client.PostAsync($"/parcel/{trackingId.trackingId}/webhooks?url={testUrl}", null);
            var jsonBody2 = await res2.Content.ReadAsStringAsync();

            var webhookResponse = JsonConvert.DeserializeObject<WebhookResponse>(jsonBody2);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res2.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res2.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");
            Assert.AreEqual(trackingId.trackingId, webhookResponse.TrackingId);
            Assert.AreEqual(testUrl, webhookResponse.Url);
        }

        [Order(9), Test, Description("Gets all Webhooks")]
        public async Task GetWebhooks()
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

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}/webhooks x2");

            var res2 = await client.PostAsync($"/parcel/{trackingId.trackingId}/webhooks?url={testUrl}", null);

            var res3 = await client.PostAsync($"/parcel/{trackingId.trackingId}/webhooks?url={testUrl}", null);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res2.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual(System.Net.HttpStatusCode.OK, res3.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res3.Content.Headers.ContentType.MediaType, "Mediatype not application/json");

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}/webhooks");

            var resMsg = await client.GetAsync($"/parcel/{trackingId.trackingId}/webhooks");
            var jsonBody2 = await resMsg.Content.ReadAsStringAsync();

            Assert.AreEqual(System.Net.HttpStatusCode.OK, resMsg.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", resMsg.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");
            Assert.That(jsonBody2, Contains.Substring($"\"trackingId\":\"{trackingId.trackingId}\""), "Sample Text not within the JSON");
            Assert.That(jsonBody2, Contains.Substring($"\"url\":\"{testUrl}\""), "Sample Text not within the JSON");
        }

        [Order(10), Test, Description("Deletes a webhook by Id")]
        public async Task DeleteWebhook()
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

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}/webhooks");

            var res2 = await client.PostAsync($"/parcel/{trackingId.trackingId}/webhooks?url={testUrl}", null);
            var jsonBody2 = await res2.Content.ReadAsStringAsync();

            var webhookResponse = JsonConvert.DeserializeObject<WebhookResponse>(jsonBody2);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res2.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res2.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.IsNotEmpty(jsonBody2, "No body returned");

            Console.WriteLine($"Calling {url}/parcel/webhooks/{webhookResponse.Id}");

            var res3 = await client.DeleteAsync($"/parcel/webhooks/{webhookResponse.Id}");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res3.StatusCode, "Status Code not 200-OK");

            Console.WriteLine($"Calling {url}/parcel/{trackingId.trackingId}/webhooks");

            var resMsg = await client.GetAsync($"/parcel/{trackingId.trackingId}/webhooks");
            var jsonBody4 = await resMsg.Content.ReadAsStringAsync();

            Assert.AreEqual(System.Net.HttpStatusCode.OK, res.StatusCode, "Status Code not 200-OK");
            Assert.AreEqual("application/json", res.Content.Headers.ContentType.MediaType, "Mediatype not application/json");
            Assert.AreEqual("[]", jsonBody4);
            Assert.That(jsonBody4, !Contains.Substring($"\"id\":\"{webhookResponse.Id}\""), "Sample Text not within the JSON");
        }
    }
}
