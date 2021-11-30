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

namespace SKSGroupF.SKS.Package.DataAccess.Tests
{
    class SqlWebhookRepositoryTests
    {
        private IWebhookRepository repo;
        DALWebhookResponses responses;

        [SetUp]
        public void Setup()
        {
            responses = new DALWebhookResponses();
            responses.Add(Builder<DALWebhookResponse>.CreateNew().Build());
            responses.Add(Builder<DALWebhookResponse>.CreateNew().Build());
            responses.Add(Builder<DALWebhookResponse>.CreateNew().Build());

            var DBMock = new Mock<ISqlDbContext>();
            DBMock.Setup(p => p.DbWebhooks).Returns(SqlDbContextMock.GetQueryableMockDbSet(responses));

            repo = new SqlWebhookRepository(DBMock.Object, new NullLogger<SqlWebhookRepository>());
        }
    }
}
