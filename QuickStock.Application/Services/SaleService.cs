using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Repositories;

namespace QuickStock.Application.Services
{
    public class SaleService
    {
        private readonly SaleRepository _saleRepository;
        private readonly ProductRepository _productRepository;

        public SaleService(
            SaleRepository saleRepository,
            ProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> CreateSaleAsync(Sale sale)
        {
            await _saleRepository.AddAsync(sale);
            return true;
        }
    }
}
