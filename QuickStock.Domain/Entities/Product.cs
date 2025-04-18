using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStock.Domain.Entities
{
    public class Product
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        public string size { get; set; }

        [Required]
        [StringLength(100)]
        public string color { get; set; }

        public int stock { get; set; }

        public decimal price { get; set; }


    }
}
