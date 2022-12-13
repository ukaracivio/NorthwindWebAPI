using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace NorthwindWebAPI.Models
{
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        [Key]
        [DisplayName("Bölge Kodu")]
        public int RegionId { get; set; }
        [Required]
        [DisplayName("Bölge Tanımı")]
        public string RegionDescription { get; set; } = null!;

        public virtual ICollection<Territory> Territories { get; set; }
    }
}
