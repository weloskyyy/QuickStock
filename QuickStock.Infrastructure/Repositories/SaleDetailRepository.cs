using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Data;

namespace QuickStock.Infrastructure.Repositories
{
    public class SaleDetailRepository
    {
        private readonly QuickStockDbContext _context;

        public SaleDetailRepository(QuickStockDbContext context)
        {
            _context = context;
        }

        public async Task<List<SaleDetail>> GetAllAsync()
        {
            return await _context.SaleDetails.Include(sd => sd.Product).Include(sd => sd.Sales).ToListAsync();
        }

        public async Task<SaleDetail?> GetByIdAsync(int id)
        {
            return await _context.SaleDetails.Include(sd => sd.Product).Include(sd => sd.Sales).FirstOrDefaultAsync(sd => sd.Id == id);
        }

        public async Task AddAsync(SaleDetail saleDetail)
        {
            await _context.SaleDetails.AddAsync(saleDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SaleDetail saleDetail)
        {
            _context.SaleDetails.Update(saleDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var saleDetail = await _context.SaleDetails.FindAsync(id);
            if (saleDetail != null)
            {
                _context.SaleDetails.Remove(saleDetail);
                await _context.SaveChangesAsync();
            }
        }
    }
}