using AtaRK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.Data
{
    public class ServerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ClimateDevice> ClimateDevices { get; set; }
        public DbSet<ClimateState> ClimateStates { get; set; }
        public DbSet<FastFoodFranchise> FastFoodFranchises { get; set; }
        public DbSet<FranchiseContactInfo> FranchiseContactInfos { get; set; }
        public DbSet<FranchiseImage> FranchiseImages { get; set; }
        public DbSet<FranchiseShop> FranchiseShops { get; set; }
        public DbSet<ShopAdmin> ShopAdmins { get; set; }
        public DbSet<ShopApplication> ShopApplications { get; set; }
        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<TechMessage> TechMessages { get; set; }
        public DbSet<TechMessageAnswer> TechMessageAnswers { get; set; }

        public ServerDbContext(DbContextOptions<ServerDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<FastFoodFranchise>().ToTable("FastFoodFranchises");
            modelBuilder.Entity<FranchiseShop>().ToTable("FranchiseShops");
            modelBuilder.Entity<ShopAdmin>().ToTable("ShopAdmins");
            modelBuilder.Entity<SystemAdmin>().ToTable("SystemAdmins");
        }
    }
}
