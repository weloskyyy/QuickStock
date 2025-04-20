using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStock.Application.DTOS
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

}
