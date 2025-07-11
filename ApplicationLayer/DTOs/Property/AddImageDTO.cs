using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Property
{
   public  class AddImageDTO
    {
      
        public bool MainImage { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
