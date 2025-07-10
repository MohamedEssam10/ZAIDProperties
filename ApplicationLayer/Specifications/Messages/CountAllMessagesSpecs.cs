using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.Messages
{
    public class CountAllMessagesSpecs : BaseSpecification<Message>
    {
        public CountAllMessagesSpecs(SpecParams Params)
        {
            Criteria = M =>
                (string.IsNullOrEmpty(Params.Search) || M.SenderName.Contains(Params.Search) || M.Body.Contains(Params.Search));
        }
    }
}
