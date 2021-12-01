using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    [ExcludeFromCodeCoverage]
    public class SqlDbContext : DbContext, ISqlDbContext
    {
        private readonly ILogger logger;

        public SqlDbContext(DbContextOptions<SqlDbContext> options, ILogger<SqlDbContext> logger) : base(options)
        {
            this.logger = logger;
            this.Database.EnsureDeleted();

            try
            {
                //this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                string errorMsg = "Failed to connect to database.";
                logger.LogError(errorMsg);
                throw new DALConnectionException(nameof(SqlDbContext), nameof(SqlDbContext), errorMsg, ex);
            }
        }

        public virtual DbSet<DALParcel> DbParcel { get; set; }
        public virtual DbSet<DALReceipient> DbReceipient { get; set; }
        public virtual DbSet<DALHop> DbHop { get; set; }
        public virtual DbSet<DALWarehouse> DbWarehouse { get; set; }
        public virtual DbSet<DALTruck> DbTruck { get; set; }
        public virtual DbSet<DALTransferWarehouse> DbTransferWarehouse { get; set; }
        public virtual DbSet<DALWarehouseNextHops> DbWarehouseNextHops { get; set; }
        public virtual DbSet<DALHopArrival> DbHopArrival { get; set; }
        public virtual DbSet<DALGeoCoordinate> DbGeoCoordinate { get; set; }
        public virtual DbSet<DALWebhookResponse> DbWebhooks { get; set; }

        public int SaveChangesToDb()
        {
            return SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            try
            {
                builder.Entity<DALParcel>().HasKey(p => p.Id);

                builder.Entity<DALReceipient>().HasKey(p => p.Id);

                builder.Entity<DALHop>().HasKey(p => p.Id);
                builder.Entity<DALHop>().HasIndex(q => q.Code).IsUnique();

                builder.Entity<DALHopArrival>().HasKey(p => p.Id);

                builder.Entity<DALGeoCoordinate>().HasKey(p => p.Id);

                builder.Entity<DALWarehouseNextHops>().HasKey(p => p.Id);

                builder.Entity<DALWebhookResponse>().HasKey(p => p.Id);
            }
            catch (Exception ex)
            {
                string errorMsg = "Error occured when trying to create DB model.";
                logger.LogError(errorMsg);
                throw new DALSqlContextException(nameof(SqlDbContext), nameof(OnModelCreating), errorMsg, ex);
            }
        }
    }
}
