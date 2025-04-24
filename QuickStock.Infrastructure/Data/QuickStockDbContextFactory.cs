using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuickStock.Infrastructure.Data;

namespace QuickStock.Infrastructure
{
    public class QuickStockDbContextFactory : IDesignTimeDbContextFactory<QuickStockDbContext>
    {
        public QuickStockDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuickStockDbContext>();

            optionsBuilder.UseSqlServer("Server=WELOSKKYY\\MSSQLSERVER02;Database=QuickStockDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new QuickStockDbContext(optionsBuilder.Options);
        }
    }
}
