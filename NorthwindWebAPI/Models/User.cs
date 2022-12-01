using System;
using System.Collections.Generic;

namespace NorthwindWebAPI.Models
{
    public partial class User
    {
        public byte UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPass { get; set; } = null!;
        public byte[]? UserPassHash { get; set; }
        public byte[]? UserPassSalt { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
