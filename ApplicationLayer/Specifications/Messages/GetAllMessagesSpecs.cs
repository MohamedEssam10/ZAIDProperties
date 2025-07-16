using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.Messages
{
    public class GetAllMessagesSpecs : BaseSpecification<Message>
    {
        public GetAllMessagesSpecs(MessageSpecParams Params)
        {
            Criteria = M =>
                ((string.IsNullOrEmpty(Params.Search) || M.SenderName.Contains(Params.Search) || M.Body.Contains(Params.Search))
                &&
                (!Params.IsRead.HasValue || M.IsRead == Params.IsRead));

            if (Params.Sort is not null && Params.Sort.Count != 0)
            {
                foreach(var sort in Params.Sort)
                {
                    switch (sort)
                    {
                        case "isread:asc":
                            AddOrderBy(M => M.IsRead);
                            break;
                        case "isread:desc":
                            AddOrderBy(M => M.IsRead, true);
                            break;
                        case "date:asc":
                            AddOrderBy(M => M.CreatedAt);
                            break;
                        case "date:desc":
                            AddOrderBy(M => M.CreatedAt, true);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                AddOrderBy(M => M.IsRead);
                AddOrderBy(M => M.CreatedAt, true);
            }

            IsPaginated = true;
            ApplyPagination((Params.PageNumber - 1) * Params.PageSize, Params.PageSize);
        }
    }
}
