using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using SKSGroupF.SKS.Package.ServiceAgents.Entities;
using SKSGroupF.SKS.Package.ServiceAgents.Interfaces;

namespace SKSGroupF.SKS.Package.ServiceAgents.Tests
{
    public class OSMGeoEncodingAgentTests
    {
        SAReceipient receipient;
        OSMGeoEncodingAgent agent;

        [SetUp]
        public void Setup()
        {
            receipient = Builder<SAReceipient>.CreateNew()
                .With(p => p.Country = "Austria")
                .With(p => p.City = "Vienna")
                .With(p => p.PostalCode = "1200")
                .With(p => p.Street = "Hochstadtplatz 3")
                .Build();

            agent = new OSMGeoEncodingAgent(new NullLogger<OSMGeoEncodingAgent>(), new System.Net.Http.HttpClient());
        }

        [Test]
        public void ParseUrl_ReceivesReceipient_ReturnsUrlWithReceipientData()
        {
            string expected = $"search?format=json&street={receipient.Street}&postalcode={receipient.PostalCode}&city={receipient.City}&country={receipient.Country}";

            string actual = agent.ParseUrlRessource(receipient);

            Assert.AreEqual(expected, actual);
        }
    }
}