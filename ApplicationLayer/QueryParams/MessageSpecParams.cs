using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.QueryParams
{
    public class MessageSpecParams : SpecParams
    {
        public List<string>? Sort { get; set; }

        public bool? IsRead { get; set; }
    }
}
