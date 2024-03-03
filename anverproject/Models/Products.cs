using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trendyol.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(30), RegularExpression("^[A-Z][a-z]+$")]
        public string Name { get; set; }
        [Required, MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public int Count { get; set; }
    }
}
