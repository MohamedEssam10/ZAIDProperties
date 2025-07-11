using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Specifications.Properties
{
    public class GetAllPropertySpecs : BaseSpecification<Property>
    {
        public GetAllPropertySpecs(PropertyParams param)
        {
            Criteria = p =>
                (!param.Type.HasValue || p.Type == param.Type) &&
                (!param.Status.HasValue || p.Status == param.Status) &&
                (string.IsNullOrEmpty(param.Location) || p.Location.Contains(param.Location)) &&
                (!param.MinPrice.HasValue || p.Price >= param.MinPrice) &&
                (!param.MaxPrice.HasValue || p.Price <= param.MaxPrice) &&
                (!param.MinArea.HasValue || p.Area >= param.MinArea) &&
                (!param.MaxArea.HasValue || p.Area <= param.MaxArea);

            AddOrderBy(p => Guid.NewGuid());

            if (param.PageNumber > 0 && param.PageSize > 0)
            {
                ApplyPagination((param.PageNumber - 1) * param.PageSize, param.PageSize);
            }

            AddInclude(p => p.Include(i => i.Images.Where(i=>i.IsMainImage)));

        }
    }
}
