using System.ComponentModel.DataAnnotations;

namespace NorthwindWebAPI.ViewModels
{
    public class RegisterVM
    {
        [StringLength(10)]
        public string? UserName { get; set; }

        [StringLength(200)]
        public string? UserPass { get; set; }

        [StringLength(5)]
        public string? CustomerID { get; set; }
    }
}
