using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces
{
    public interface ISqlDbContext
    {
        public DbSet<DALParcel> DbParcel { get; set; }
        public DbSet<DALReceipient> DbReceipient { get; set; }
        public DbSet<DALHop> DbHop { get; set; }
        public DbSet<DALWarehouse> DbWarehouse { get; set; }
        public DbSet<DALTruck> DbTruck { get; set; }
        public DbSet<DALTransferWarehouse> DbTransferWarehouse { get; set; }
        public DbSet<DALHopArrival> DbHopArrival { get; set; }
        public DbSet<DALGeoCoordinate> DbGeoCoordinate { get; set; }
        public DbSet<DALWarehouseNextHops> DbWarehouseNextHops { get; set; }

        int SaveChangesToDb();
    }
}