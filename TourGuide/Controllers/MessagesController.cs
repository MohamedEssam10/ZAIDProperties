using ApplicationLayer.Contracts.Services;
using ApplicationLayer.DTOs.Message;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageServices messageServices;

        public MessagesController(IMessageServices messageServices)
        {
            this.messageServices = messageServices;
        }

        [Authorize]
        [HttpGet("GetAllMessages")]
        public async Task<ActionResult<APIResponse<Pagination<MessageDTOResponse>>>> GetAllMessages([FromQuery] MessageSpecParams Params)
        {
            var response = await messageServices.GetAll(Params);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("GetMessageById/{id}")]
        public async Task<ActionResult<APIResponse<MessageDTOResponse>>> GetMessageById(int id)
        {
            var response = await messageServices.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<APIResponse<string>>> DeleteMessage(int id)
        {
            var response = await messageServices.Delete(id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<APIResponse<string>>> MarkAsRead(int id)
        {
            var response = await messageServices.MarkAsRead(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<string>>> Add(MessageDTORequest messageDTO)
        {
            var response = await messageServices.Add(messageDTO);
            return StatusCode(response.StatusCode, response);
        }
    }
}
