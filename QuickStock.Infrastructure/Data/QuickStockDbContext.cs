

using Microsoft.EntityFrameworkCore;
using QuickStock.Domain.Entities;

namespace QuickStock.Infrastructure.Data
{
    public class QuickStockDbContext : DbContext
    {
        public QuickStockDbContext(DbContextOptions<QuickStockDbContext> Options) : base(Options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        
    }
}