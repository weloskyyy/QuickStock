using System.ComponentModel.DataAnnotations;

namespace QuickStock.Application.DTOs
{
    public class ProductUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La talla es obligatoria")]
        public string Size { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Color { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int CategoryId { get; set; }
    }
}
