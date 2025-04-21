using System.ComponentModel.DataAnnotations;

namespace QuickStock.Application.DTOS
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
