using System.ComponentModel.DataAnnotations;

namespace QuickStock.Application.DTOS
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Name { get; set; } = string.Empty;
    }
}
