using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIREST_GET.Models
{
    public partial class User
    {
        [Key]
        public Guid IdeUser { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? Password { get; set; }
        [Required]
        public string? Rol { get; set; }
    }
}
