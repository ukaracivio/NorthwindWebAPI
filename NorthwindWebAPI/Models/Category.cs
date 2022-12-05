using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthwindWebAPI.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Bu bilgi boş bırakılamaz...Lütfen kontrol ediniz...")]
        [StringLength(15,ErrorMessage ="Bu bilgi en fazla {1} karakter uzunluğunda olmalıdır...")]
        [DisplayName("Kategori")]
        public string CategoryName { get; set; } = null!;
        [DisplayName("Kategori Tanımı")]
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
