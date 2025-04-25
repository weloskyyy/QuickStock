using Microsoft.EntityFrameworkCore;
using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickStock.Infrastructure.Repositories
{
    public class SaleRepository
    {
        private readonly QuickStockDbContext _context;

        public SaleRepository(QuickStockDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _context.Sales
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Sale sale)
        {
            
            sale.TotalAmount = sale.Quantity * sale.UnitPrice;

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            
            sale.TotalAmount = sale.Quantity * sale.UnitPrice;

            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }
    }
}