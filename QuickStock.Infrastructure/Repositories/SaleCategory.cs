using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickStock.Domain.Entities;
using QuickStock.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace QuickStock.infrastructure.Repositories
{
    public class SaleRepository
    {
        private readonly QuickStockDbContext _context;

        public SaleRepository(QuickStockDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales.Include(s => s.SaleDetails).ToListAsync();
        }
        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _context.Sales.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
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
