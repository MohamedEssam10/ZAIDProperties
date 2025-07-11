using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.QueryParams
{
   public class PropertyParams  
    {


        public List<string> Sort = new List<string>() { "isread:asc", "date:desc" };

        private int pageNumber = 1;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value <= 0 ? 1 : value; }
        }

        private int pageSize = 50;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 50 || value <= 0 ? 50 : value; }
        }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.Trim().ToLower(); }
        }
        public bool IsPageable = true;

        public PropertyType? Type { get; set; }
        public PropertyStatus? Status { get; set; }
        public string? Location { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; } // Added this property
        public int? MinArea { get; set; }
        public int? MaxArea { get; set; }
     
        public decimal Price { get; set; }
        public int Area { get; set; }

    }
}
