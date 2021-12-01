using Microsoft.EntityFrameworkCore;
using Moq;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SKSGroupF.SKS.Package.DataAccess.Tests
{
    public static class SqlDbContextMock
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(e => e.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));

            if (typeof(T) == typeof(DALParcel))
            {
                dbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(x => (sourceList as List<DALParcel>).FirstOrDefault(y => y.Id == (int)x[0]) as T);
            }
            else if (typeof(T) == typeof(DALHop))
            {
                dbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(x => (sourceList as List<DALHop>).FirstOrDefault(y => y.Id == (int)x[0]) as T);
            }
            else if (typeof(T) == typeof(DALReceipient))
            {
                dbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(x => (sourceList as List<DALReceipient>).FirstOrDefault(y => y.Id == (int)x[0]) as T);
            }
            else if (typeof(T) == typeof(DALHopArrival))
            {
                dbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(x => (sourceList as List<DALHopArrival>).FirstOrDefault(y => y.Id == (int)x[0]) as T);
            }
            else if (typeof(T) == typeof(DALWebhookResponse))
            {
                dbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(x => (sourceList as List<DALWebhookResponse>).FirstOrDefault(y => y.Id == (long?)x[0]) as T);
            }

            return dbSet.Object;
        }
    }
}