using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStock.Domain.Entities
{
    public class SaleDetail
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        [Required]
        public Sale Sales { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}