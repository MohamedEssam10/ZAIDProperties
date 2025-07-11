using ApplicationLayer.Models;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.Properties
{
   public class GetPropertyById :BaseSpecification<Property>
    {
        public GetPropertyById(int Id)
        {
            Criteria=P=>P.Id==Id;
        }
    }
}
