using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.ServiceAgents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SKSGroupF.SKS.Package.ServiceAgents.Entities;

namespace SKSGroupF.SKS.Package.ServiceAgents
{
    public class OSMGeoEncodingAgent : IGeoEncodingAgent
    {
        private readonly HttpClient client;
        private readonly ILogger logger;

        public OSMGeoEncodingAgent(ILogger<OSMGeoEncodingAgent> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
            client.DefaultRequestHeaders.Add("User-Agent", "SKSGroupF");
        }

        public SAGeoCoordinate EncodeAddress(SAReceipient receipient)
        {
            string url = ParseUrlRessource(receipient);

            Task<string> task = Task.Run<string>(async () => await GetDataFromAPI(url));
            string responseString = task.Result;

            List<SAGeoCoordinate> coor = JsonConvert.DeserializeObject<List<SAGeoCoordinate>>(responseString);
            return coor[0];
        }

        async Task<string> GetDataFromAPI(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public string ParseUrlRessource(SAReceipient rec)
        {
            string url = $"search?format=json&street={rec.Street}&postalcode={rec.PostalCode}&city={rec.City}&country={rec.Country}";
            return url;
        }
    }
}
