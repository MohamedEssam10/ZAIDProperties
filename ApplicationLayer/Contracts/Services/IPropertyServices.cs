using ApplicationLayer.DTOs.Property;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface IPropertyServices
    {
        Task<APIResponse<List<PropertyDTOResponse>>> GetAll(PropertyParams Params);
       Task<APIResponse<PropertyDTOResponse>> GetbyId(int Id);

       Task<APIResponse<PropertyDTOAdd>> AddProperty(PropertyDTOAdd propertyDTOAdd);

       Task<APIResponse<PropertyDTOUpdate>>UpdateProperty(PropertyDTOUpdate propertyDTOUpdate);

         Task<APIResponse<PropertyDTOResponse>> DeleteProperty(int Id);


    }
}
