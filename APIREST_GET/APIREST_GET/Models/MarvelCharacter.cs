using System;
using System.Collections.Generic;

namespace APIREST_GET.Models
{
    public partial class MarvelCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public int ConsultNumber { get; set; }
    }
}