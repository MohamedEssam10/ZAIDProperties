using DomainLayer.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Property
{
   public class PropertyDTOResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; }
        public string Location { get; set; } = null!;
        public decimal Price { get; set; }
        public int Area { get; set; }
        public string? Images { get; set; } = null!;
    }
}
