using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickStock.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Name { get; set; } = string.Empty; // ✅ Evita advertencias

        public ICollection<Product> Products { get; set; } = new List<Product>(); // ✅ Inicialización segura
    }
}
