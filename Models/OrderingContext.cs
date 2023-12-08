using Microsoft.EntityFrameworkCore;
namespace CampusOrdering.Models

{
    public class OrderingContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems{ get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        public OrderingContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().ToTable("Restaurant");
            modelBuilder.Entity<MenuItem>().ToTable("MenuItem");
            modelBuilder.Entity<Receipt>().ToTable("Receipt");
        }
    }
}
