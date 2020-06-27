using JemenaGasMeter.WebApi.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace JemenaGasMeter.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { Database.EnsureCreated(); }

        public DbSet<User> Users { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Installation> Installations { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<MeterHistory> MeterHistories { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MeterHistory>().
                HasOne(x => x.Meter).WithMany(x => x.MeterHistories).HasForeignKey(x => x.MIRN);
            builder.Entity<MeterHistory>().
                HasOne(x => x.User).WithMany(x => x.MeterHistories).HasForeignKey(x => x.PayRollID);

            base.OnModelCreating(builder);
        }
    }
}
