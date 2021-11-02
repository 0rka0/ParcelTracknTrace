using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlDbContext : DbContext, ISqlDbContext
    {
        private readonly string connectionString;
        private IConfiguration config;

        public SqlDbContext()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
            this.connectionString = $"{config["SqlDbContext"]}";
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<DALParcel> DbParcel { get; set; }
        public DbSet<DALReceipient> DbReceipient { get; set; }
        public DbSet<DALHop> DbHop { get; set; }
        public DbSet<DALWarehouse> DbWarehouse { get; set; }
        public DbSet<DALTruck> DbTruck { get; set; }
        public DbSet<DALTransferWarehouse> DbTransferWarehouse { get; set; }
        public DbSet<DALHopArrival> DbHopArrival { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DALParcel>(entitiy =>
            {
                entitiy.HasKey(p => p.Id);
            });

            builder.Entity<DALReceipient>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            builder.Entity<DALHop>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            builder.Entity<DALHopArrival>();
        }

        public int SaveChangesToDb()
        {
            return SaveChanges();
        }
    }
}
