using Microsoft.EntityFrameworkCore;
using QuickStock.Domain.Entities;

namespace QuickStock.Infrastructure.Data
{
    public class QuickStockDbContext : DbContext
    {

        public QuickStockDbContext(DbContextOptions<QuickStockDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);
        }
    }
}