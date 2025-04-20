

namespace QuickStock.Domain.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Decimal Total { get; set; }

       

    }
}
