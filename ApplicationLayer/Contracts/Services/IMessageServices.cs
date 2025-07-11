using ApplicationLayer.DTOs.Message;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Services
{
    public interface IMessageServices
    {
        Task<APIResponse<Pagination<MessageDTOResponse>>> GetAll(MessageSpecParams Params);
        Task<APIResponse<MessageDTOResponse>> GetById(int id);
        Task<APIResponse<string>> Add(MessageDTORequest message);
        Task<APIResponse<string>> Delete(int id);
        Task<APIResponse<string>> MarkAsRead(int id);
    }
}
