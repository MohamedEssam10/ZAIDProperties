using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Property;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        public IPropertyServices PropertyServices { get; }
        public PropertyController(IPropertyServices propertyServices)
        {
            PropertyServices = propertyServices;
        }
        [HttpGet("GetAllProperties")]
        public async Task<ActionResult<APIResponse<List<PropertyDTOResponse>>>> GetAllProperties([FromQuery] PropertyParams Params)
        {
            var response = await PropertyServices.GetAll(Params);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetPropertyById/{Id}")]
        public async Task<IActionResult> GetPropertyById(int Id)
        {
            var response = await PropertyServices.GetbyId(Id);

            if (!response.Succeeded)
                return StatusCode(response.StatusCode, response);


            return Ok(response);
        }


        [HttpPost("AddProperty")]
        public async Task<ActionResult<APIResponse<PropertyDTOAdd>>> AddProperty([FromForm] PropertyDTOAdd propertyDTOAdd)
        {
            var response = await PropertyServices.AddProperty(propertyDTOAdd);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPut("UpdateProperty/{Id}")]
        public async Task<ActionResult<APIResponse<PropertyDTOUpdate>>> UpdateProperty([FromForm] PropertyDTOUpdate propertyDTOUpdate)
        {

            var response = await PropertyServices.UpdateProperty(propertyDTOUpdate);
            return StatusCode(response.StatusCode, response);

        }

        [HttpDelete("DeleteProperty/{Id}")]
        public async Task<ActionResult<APIResponse<PropertyDTOResponse>>> DeleteProperty(int Id)
        {
            var response = await PropertyServices.DeleteProperty(Id);
            if (!response.Succeeded)
                return StatusCode(response.StatusCode, response);
            return Ok(response);

        }



    }

 
}
