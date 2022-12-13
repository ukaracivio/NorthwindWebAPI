using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NorthwindWebAPI.Models
{
    public partial class Territory
    {
        public Territory()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        [DisplayName("Şehir Kodu")]
        public string TerritoryId { get; set; } = null!;
        [DisplayName("Şehir Adı")]
        public string TerritoryDescription { get; set; } = null!;

        public int? RegionId { get; set; }

        public virtual Region? Region { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
