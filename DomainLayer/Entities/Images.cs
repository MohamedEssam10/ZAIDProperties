using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Images
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsMainImage { get; set; } = false;
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}
