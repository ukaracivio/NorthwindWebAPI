using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NorthwindWebAPI.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            CustomerTypes = new HashSet<CustomerDemographic>();
        }

        [Key]
        [Required(ErrorMessage = "Bu bilgi boş bırakılamaz...Lütfen kontrol ediniz...")]
        [StringLength(5, ErrorMessage = "Bu bilgi en fazla {1} karakter uzunluğunda olmalıdır...")]

        public string CustomerId { get; set; } = null!;

        [Required(ErrorMessage = "Bu bilgi boş bırakılamaz...Lütfen kontrol ediniz...")]
        [StringLength(40, ErrorMessage = "Bu bilgi en fazla {1} karakter uzunluğunda olmalıdır...")]
        public string CompanyName { get; set; } = null!;
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? UserName { get; set; }
        public string? UserPass { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<CustomerDemographic> CustomerTypes { get; set; }
    }
}
