using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Enums
{
    public enum PropertyType
    {
        شقة,
        أراضي,
        فيلا,
        شاليه,
        عمارة,
        [Display(Name = "بيت شخصي")]
        بيت_شخصي,
        مكتب,
        محل,
        مخزن,
    }
}
