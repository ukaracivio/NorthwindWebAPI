using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NorthwindWebAPI.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int SupplierId { get; set; }
        [Required]
        [DisplayName("Tedarikçi")]
        public string CompanyName { get; set; } = null!;
        [DisplayName("Sorumlu Kişi")]
        public string? ContactName { get; set; }
        [DisplayName("Ünvan")]
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? HomePage { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
