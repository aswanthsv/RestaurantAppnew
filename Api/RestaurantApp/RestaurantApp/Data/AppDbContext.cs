using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantApp.DTOs;
using RestaurantApp.Models;

namespace RestaurantApp.Data
{
    public class AppDbContext: DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Models.Table> Tables { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Personal> Personal { get; set; }

        public DbSet<DailySalesSummaryDto> DailySalesSummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<DailySalesSummaryDto>().HasNoKey();

        }

    }
}
