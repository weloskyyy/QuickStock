using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Repositories;

namespace QuickStock.Application.Services
{
    public class SaleService
    {
        private readonly SaleRepository _saleRepository;
        private readonly ProductRepository _productRepository;
        private readonly SaleDetailRepository _saleDetailRepository;

        public SaleService(
            SaleRepository saleRepository,
            ProductRepository productRepository,
            SaleDetailRepository saleDetailRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _saleDetailRepository = saleDetailRepository;
        }

        public async Task<bool> CreateSaleAsync(Sale sale, List<SaleDetail> saleDetails)
        {
            await _saleRepository.AddAsync(sale);

            foreach (var detail in saleDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductId);
                if (product == null || product.Stock < detail.Quantity)
                {
                    throw new Exception($"El producto no es valido o no se ecuentra en estos momentos{detail.ProductId}.");
                }

                product.Stock -= detail.Quantity;
                await _productRepository.UpdateAsync(product);

                detail.SaleId = sale.Id;
                await _saleDetailRepository.AddAsync(detail);
            }

            return true;
        }
    }
}
