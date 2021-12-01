using NUnit.Framework;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SKSGroupF.SKS.Package.DataAccess.Sql;
using Microsoft.Extensions.Logging.Abstractions;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;

namespace SKSGroupF.SKS.Package.DataAccess.Tests
{
    class SqlWebhookRepositoryTests
    {
        private IWebhookRepository repo;
        DALWebhookResponses responses;
        DALWebhookResponse validResponse;

        [SetUp]
        public void Setup()
        {
            responses = new DALWebhookResponses();
            responses.Add(Builder<DALWebhookResponse>.CreateNew().With(p => p.Id = 1).Build());
            responses.Add(Builder<DALWebhookResponse>.CreateNew().With(p => p.Id = 2).Build());
            responses.Add(Builder<DALWebhookResponse>.CreateNew().With(p => p.Id = 3).Build());

            validResponse = Builder<DALWebhookResponse>.CreateNew().Build();
            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbWebhooks).Returns(SqlDbContextMock.GetQueryableMockDbSet(responses));
            DBMock.Setup(p => p.SaveChangesToDb()).Returns(1);
            
            repo = new SqlWebhookRepository(DBMock.Object, new NullLogger<SqlWebhookRepository>());
        }

        [Test]
        public void Create_InsertSubscriptionIntoDB_IncreasesDbCountByOne()
        {
            var counter = responses.Count + 1;
            try
            {
                repo.Create(validResponse);
            }
            catch { }

            Assert.AreEqual(counter, responses.Count);
        }

        [Test]
        public void Create_InsertsResponseIntoDB_InsertsResponseCorrectly()
        {
            validResponse.Id = 3;
            validResponse.TrackingId = "fffff";
            var expected = validResponse.TrackingId;
                    
            try
            {
                repo.Create(validResponse);
            }
            catch 
            {     
            }

            Assert.AreEqual(expected, responses.Last().TrackingId);
        }

        [Test]
        public void Delete_DeletesResponseWithId_DecreasesDbCountByOne()
        {
            var counter = responses.Count - 1;
            repo.Delete(1);
            Assert.AreEqual(counter, responses.Count);
        }

        [Test]
        public void Delete_DeletesResponseWithId_CorrectResponseRemovedFromDb()
        {
            var expected1 = 2;

            repo.Delete(1);

            Assert.AreEqual(expected1, responses[0].Id);
        }

        [Test]
        public void Delete_CannotDeleteSpecifiedResponse_ThrowsDataNotFoundException()
        {
            Assert.Throws<DALDataNotFoundException>(() => repo.Delete(20));
        }

        [Test] 
        public void GetAll_SelectsAllResponsesFromDb_SelectsResponseCorrectly()
        {
            var responseList = repo.GetAll();
            Assert.AreEqual(responses.Count, responseList.ToList().Count);
        }

        [Test]
        public void GetAll_CannotFindAnyResponse_ThrowsDataException()
        {
            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbWebhooks).Throws(new Exception());

            repo = new SqlWebhookRepository(DBMock.Object, new NullLogger<SqlWebhookRepository>());

            Assert.Throws<DALDataException>(() => repo.GetAll());
        }

        [Test]
        public void GetAllWithTrackingId_SelectsAllResponsesWithSameTrackingIdFromDb_SelectsResponseCorrectly()
        {
            var responseList = repo.GetAllWithTrackingId(responses[1].TrackingId);
            Assert.AreEqual(responses.Count, responseList.ToList().Count);
        }

        [Test]
        public void GetAllWithTrackingId_CannotFindAnyResponse_ThrowsDataException()
        {
            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbWebhooks).Throws(new Exception());

            repo = new SqlWebhookRepository(DBMock.Object, new NullLogger<SqlWebhookRepository>());

            Assert.Throws<DALDataException>(() => repo.GetAllWithTrackingId(responses[1].TrackingId)); 
        }

        [Test]
        public void GetById_SelectsResponseWithValidId_SelectsCorrectResponse()
        {
            var response = repo.GetById(responses[1].Id);

            Assert.AreEqual(responses[1].Id, response.Id);
        }

        [Test]
        public void GetById_SelectsResponseWithNoneExistingId_ReturnsDataException()
        {
            Assert.Throws<DALDataException>(() => repo.GetById(123));
        }


    }
}
