using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SKSGroupF.SKS.Package.BusinessLogic.Entities.Models;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces;
using SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using SKSGroupF.SKS.Package.BusinessLogic.Logic;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.ServiceAgents;
using SKSGroupF.SKS.Package.ServiceAgents.Entities;
using SKSGroupF.SKS.Package.ServiceAgents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SKSGroupF.SKS.Package.BusinessLogic.Tests
{
    public class ParcelLogicTests
    {
        private IParcelLogic logic;
        private BLParcel validParcel;
        private BLParcel invalidParcel;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new BlDalProfiles());
            });
            mapper = config.CreateMapper();

            validParcel = Builder<BLParcel>.CreateNew()
                .With(p => p.Receipient = Builder<BLReceipient>.CreateNew()
                    .With(p => p.Country = "Austria")
                    .With(p => p.City = "Wien")
                    .With(p => p.Name = "Herbert")
                    .With(p => p.PostalCode = "A-1010")
                    .With(p => p.Street = "Landstraße 1")
                    .Build())
                .With(p => p.Sender = Builder<BLReceipient>.CreateNew()
                    .With(p => p.Country = "Austria")
                    .With(p => p.City = "Wien")
                    .With(p => p.Name = "Ulrik")
                    .With(p => p.PostalCode = "A-1010")
                    .With(p => p.Street = "Industriestraße 1")
                    .Build())
                .With(p => p.Weight = 5.0f)
                .With(p => p.FutureHops = Builder<BLHopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.VisitedHops = Builder<BLHopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = BLParcel.StateEnum.InTransportEnum)
                .Build();

            invalidParcel = Builder<BLParcel>.CreateNew()
                .With(p => p.Receipient = Builder<BLReceipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BLReceipient>.CreateNew().Build())
                .With(p => p.Weight = 0)
                .Build();

            SAGeoCoordinate coor1 = Builder<SAGeoCoordinate>.CreateNew()
                .With(p => p.lat = 48.2026482)
                .With(p => p.lon = 16.3844424)
                .Build();

            SAGeoCoordinate coor2 = Builder<SAGeoCoordinate>.CreateNew()
                .With(p => p.lat = 48.2355388)
                .With(p => p.lon = 16.438514)
                .Build();

            Mock<IGeoEncodingAgent> mockAgent = new();
            mockAgent.SetupSequence(m => m.EncodeAddress(It.IsAny<ServiceAgents.Entities.SAReceipient>()))
                .Returns(coor1)
                .Returns(coor2);

            Mock <IParcelRepository> mockRepo = new();
            mockRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Models.DALParcel>())).Returns(1);
            mockRepo.Setup(m => m.Update(It.IsAny<DataAccess.Entities.Models.DALParcel>()));

            var wh = Builder<DALWarehouse>.CreateNew()
                .With(p => p.Id = 1)
                .With(p => p.HopType = "Warehouse")
                .With(p => p.Code = "WENB01")
                .With(p => p.Parent = null)
                .With(p => p.LocationCoordinates = Builder<DALGeoCoordinate>.CreateNew().With(q => q.Lat = 47.247829).With(q => q.Lon = 47.247829).Build())
                .With(p => p.Level = 1)
                .Build();
            List<DALHop> hopList = new List<DALHop>();
            hopList.Add(wh);
            hopList.Add(Builder<DALTruck>.CreateNew()
                            .With(p => p.Id = 2)
                            .With(p => p.HopType = "Truck")
                            .With(p => p.Code = "WTTA080")
                            .With(p => p.Parent = wh)
                            .With(p => p.RegionGeoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"MultiPolygon\",\"coordinates\":[[[[16.4812379,48.2734323],[16.4747104,48.2753152],[16.4732342,48.2750883],[16.452885,48.2629857],[16.4501709,48.259187],[16.4470734,48.2620364],[16.4369483,48.2616298],[16.4384404,48.2595091],[16.4354998,48.2590922],[16.4304572,48.251354],[16.4282331,48.2517915],[16.4278467,48.2465125],[16.4235931,48.2460613],[16.4147812,48.2454788],[16.4225253,48.2416497],[16.4322944,48.232556],[16.4375925,48.2343655],[16.4405015,48.2335077],[16.4423398,48.2342121],[16.443556,48.2337025],[16.4497356,48.2344343],[16.4515345,48.2304614],[16.4530369,48.2298393],[16.4607051,48.2417696],[16.4711386,48.2515876],[16.4801441,48.2677857],[16.4826513,48.2707028],[16.4842694,48.2731705],[16.4812379,48.2734323]]]]}}")
                            .Build());
            hopList.Add(Builder<DALTruck>.CreateNew()
                            .With(p => p.Id = 3)
                            .With(p => p.HopType = "Truck")
                            .With(p => p.Code = "WTTA046")
                            .With(p => p.Parent = wh)
                            .With(p => p.RegionGeoJson = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"MultiPolygon\",\"coordinates\":[[[[16.3934453,48.2122206],[16.388619,48.2131431],[16.3849009,48.2113546],[16.384367,48.2090062],[16.3809793,48.2043722],[16.3766524,48.200314],[16.375214,48.1999514],[16.3751097,48.1977764],[16.3809243,48.187927],[16.3855063,48.1829183],[16.395452,48.1755276],[16.3970699,48.1753753],[16.3977419,48.1785119],[16.4034484,48.1822368],[16.4041501,48.1834519],[16.4028279,48.1847918],[16.4046185,48.185693],[16.4056884,48.1847573],[16.4128424,48.1831524],[16.4150548,48.1854531],[16.4169455,48.1847916],[16.4177599,48.1858898],[16.4219215,48.1851697],[16.4219682,48.1865064],[16.4284336,48.1851579],[16.4311119,48.18634],[16.4226973,48.1890619],[16.4132591,48.1944878],[16.4063631,48.2010604],[16.3986277,48.2032036],[16.3971945,48.2047953],[16.3958341,48.2094202],[16.3934453,48.2122206]]]]}}")
                            .Build());
            Mock<IHopRepository> mockRepo2 = new();
            mockRepo2.Setup(m => m.GetAll()).Returns(hopList);

            logic = new ParcelLogic(mapper, mockRepo.Object, mockRepo2.Object, mockAgent.Object, new NullLogger<ParcelLogic>());
        }

        [Test]
        public void SubmitParcel_ReceivesInvalidParcel_ThrowsException()
        {
            Assert.Throws<BLLogicException>(() => logic.SubmitParcel(invalidParcel));
        }

        [Test]
        public void SubmitParcel_ReceivesValidParcel_ReturnsTrackingIdOfParcel()
        {
            string actualTrackingID = logic.SubmitParcel(validParcel);

            Assert.That(actualTrackingID, Does.Match("^[A-Z0-9]{9}$"));
        }

        [Test]
        public void SubmitParcel_ReceivesParcel_ThrowsUnknownException()
        {
            Mock<IParcelRepository> mockRepo = new();
            mockRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Models.DALParcel>())).Throws(new Exception());
            Mock<IHopRepository> mockRepo2 = new();
            logic = new ParcelLogic(mapper, mockRepo.Object, mockRepo2.Object, null, new NullLogger<ParcelLogic>());

            Assert.Throws<BLLogicException>(() => logic.SubmitParcel(validParcel));
        }

        [Test]
        public void TransitionParcel_ReceivesInvalidParcel_ThrowsException()
        {
            string trackingID = "PYJRB4HZ6";

            Assert.Throws<BLLogicException>(() => logic.TransitionParcel(invalidParcel, trackingID));
        }

        [Test]
        public void TransitionParcel_ReceivesInvalidTrackingId_ThrowsException()
        {
            string trackingID = "ABCDEFGH";

            Assert.Throws<BLLogicException>(() => logic.TransitionParcel(validParcel, trackingID));
        }

        [Test]
        public void TransitionParcel_ReceivesValidData_RunsWithoutException()
        {
            string trackingID = "PYJRB4HZ6";

            logic.TransitionParcel(validParcel, trackingID);

            Assert.IsTrue(true);
        }

    }
}