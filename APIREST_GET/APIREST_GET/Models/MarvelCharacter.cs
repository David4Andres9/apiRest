using System;
using System.Collections.Generic;

namespace APIREST_GET.Models
{
    public partial class MarvelCharacter
    {
        public Guid IdeCharacter { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
    }
}