using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SKSGroupF.SKS.Package.ServiceAgents.Entities;
using SKSGroupF.SKS.Package.ServiceAgents.Interfaces;
using SKSGroupF.SKS.Package.ServiceAgents.Interfaces.Exceptions;
using System.Net.Http;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.ServiceAgents.Tests
{
    public class OSMGeoEncodingAgentTests
    {
        SAReceipient receipient;
        OSMGeoEncodingAgent agent;
        MockHttpMessageHandler mockHttp;
        HttpClient client;

        double lat = 48.2399809;
        double lon = 16.3768477;

        [SetUp]
        public void Setup()
        {
            receipient = Builder<SAReceipient>.CreateNew()
                .With(p => p.Country = "Austria")
                .With(p => p.City = "Vienna")
                .With(p => p.PostalCode = "1200")
                .With(p => p.Street = "Hochstadtplatz 3")
                .Build();

            mockHttp = new MockHttpMessageHandler();
            mockHttp.When("http://test/*").Respond("application/json", "[{\"lat\": \"48.2399809\",\"lon\": \"16.3768477\"}]");
            client = new HttpClient(mockHttp);
            client.BaseAddress = new System.Uri("http://test/abc");

            agent = new OSMGeoEncodingAgent(new NullLogger<OSMGeoEncodingAgent>(), client);
        }

        [Test]
        public void EncodeAddressAsync_ReceivesReceipient_ReturnsGeoCoordinatesWithCorrectLatAndLon()
        {
            double latExcpected = lat;
            double lonExpected = lon;

            var actual = agent.EncodeAddress(receipient);

            Assert.AreEqual(latExcpected, actual.lat);
            Assert.AreEqual(lonExpected, actual.lon);
        }

        [Test]
        public void EncodeAddressAsync_HttpClientHasInvalidURL_ThrowsSATaskException()
        {
            client.BaseAddress = new System.Uri("http://nottest/abc");

            agent = new OSMGeoEncodingAgent(new NullLogger<OSMGeoEncodingAgent>(), client);

            Assert.Throws<SATaskException>(() => agent.EncodeAddress(receipient));
        }

        [Test]
        public void ParseUrl_ReceivesReceipient_ReturnsUrlWithReceipientData()
        {
            string expected = $"search?format=json&street={receipient.Street}&postalcode={receipient.PostalCode}&city={receipient.City}&country={receipient.Country}";

            string actual = agent.ParseUrlRessource(receipient);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseUrl_InsufficientData_ThrowsDataNotFoundException()
        {
            receipient = Builder<SAReceipient>.CreateNew()
                .With(p => p.Country = null)
                .Build();

            Assert.Throws<SADataNotFoundException>(() => agent.ParseUrlRessource(receipient));
        }
    }
}