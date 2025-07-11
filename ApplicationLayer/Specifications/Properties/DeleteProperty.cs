using ApplicationLayer.Models;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.Properties
{
   public class DeleteProperty :BaseSpecification<Property>
    {
        public DeleteProperty(int Id)
        {
            Criteria=p=>p.Id==Id;
        }
    }
}
