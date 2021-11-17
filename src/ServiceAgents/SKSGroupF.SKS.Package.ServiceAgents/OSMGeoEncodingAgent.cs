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
using SKSGroupF.SKS.Package.ServiceAgents.Interfaces.Exceptions;

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
            try
            {
                logger.LogInformation("Trying to encode receipient address");
                string url = ParseUrlRessource(receipient);

                Task<string> task = Task.Run<string>(async () => await GetDataFromAPI(url));
                string responseString = task.Result;

                List<SAGeoCoordinate> coor = JsonConvert.DeserializeObject<List<SAGeoCoordinate>>(responseString);
                return coor[0];
            }
            catch(System.AggregateException ex)
            {
                string errorMsg = "Failed to run the async Task";
                logger.LogError(errorMsg);
                throw new SATaskException(nameof(OSMGeoEncodingAgent), errorMsg, ex);
            }
            catch(SAApiCallException)
            {
                string errorMsg = "Failed call the API";
                logger.LogError(errorMsg);
                throw;
            }
            catch(SADataNotFoundException)
            {
                throw;
            }
        }

        async Task<string> GetDataFromAPI(string url)
        {
            try
            {
                logger.LogInformation("Trying to get Data from API.");
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                logger.LogInformation("API access successful.");
                return responseBody;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                string errorMsg = "Failed to access API because there was an error with the Socket";
                logger.LogError(errorMsg);
                throw new SAApiCallException(nameof(OSMGeoEncodingAgent), errorMsg, ex);
            }
            catch(System.Net.Http.HttpRequestException ex)
            {
                string errorMsg = "Failed to access API because there was an error with the HTTP Request";
                logger.LogError(errorMsg);
                throw new SAApiCallException(nameof(OSMGeoEncodingAgent), errorMsg, ex);
            }
            catch(Exception ex)
            {
                string errorMsg = "Unknown error occured when trying to access API";
                logger.LogError(errorMsg);
                throw new SAApiCallException(nameof(OSMGeoEncodingAgent), errorMsg, ex);
            }
        }

        public string ParseUrlRessource(SAReceipient rec)
        {
            try
            {
                logger.LogInformation("Parsing the URL.");
                string url = $"search?format=json&street={rec.Street}&postalcode={rec.PostalCode}&city={rec.City}&country={rec.Country}";
                return url;
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to parse URL because part of the Data was not found";
                logger.LogError(errorMsg);
                throw new SADataNotFoundException(nameof(OSMGeoEncodingAgent), errorMsg, ex);
            }
        }
    }
}
