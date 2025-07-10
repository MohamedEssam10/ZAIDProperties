using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public PropertyType Type { get; set; } 
        public PropertyStatus Status { get; set; }
        public string Location { get; set; } = null!;
        public decimal Price { get; set; }
        public int Area { get; set; } 
        public List<Images> Images { get; set; } = new List<Images>();
    }
}
