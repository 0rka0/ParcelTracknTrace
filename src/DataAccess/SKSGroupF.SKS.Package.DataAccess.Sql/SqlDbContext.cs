using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    [ExcludeFromCodeCoverage]
    public class SqlDbContext : DbContext, ISqlDbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
            //this.Database.EnsureDeleted();
            //this.Database.EnsureCreated();
        }

        public virtual DbSet<DALParcel> DbParcel { get; set; }
        public virtual DbSet<DALReceipient> DbReceipient { get; set; }
        public virtual DbSet<DALHop> DbHop { get; set; }
        public virtual DbSet<DALWarehouse> DbWarehouse { get; set; }
        public virtual DbSet<DALTruck> DbTruck { get; set; }
        public virtual DbSet<DALTransferWarehouse> DbTransferWarehouse { get; set; }
        public virtual DbSet<DALWarehouseNextHops> DbWarehouseNextHops { get; set; }
        public virtual DbSet<DALHopArrival> DbHopArrival { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DALParcel>().HasKey(p => p.Id);

            builder.Entity<DALReceipient>().HasKey(p => p.Id);

            builder.Entity<DALHop>().HasKey(p => p.Id);
            builder.Entity<DALHop>().HasIndex(q => q.Code).IsUnique();

            builder.Entity<DALHopArrival>().HasKey(p => p.Id);

            builder.Entity<DALGeoCoordinate>().HasKey(p => p.Id);

            builder.Entity<DALWarehouseNextHops>().HasKey(p => p.Id);
        }

        public int SaveChangesToDb()
        {
            return SaveChanges();
        }
    }
}
