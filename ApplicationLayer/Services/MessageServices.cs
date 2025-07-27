using ApplicationLayer.Contracts.Services;
using ApplicationLayer.Contracts.UnitToWork;
using ApplicationLayer.DTOs.Message;
using ApplicationLayer.Hubs;
using ApplicationLayer.Models;
using ApplicationLayer.QueryParams;
using ApplicationLayer.Specifications.Messages;
using DomainLayer.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.Services
{
    public class MessageServices : IMessageServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<NotificationHub> hubContext;

        public MessageServices(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        public async Task<APIResponse<Pagination<MessageDTOResponse>>> GetAll(MessageSpecParams Params)
        {
            var Specs = new GetAllMessagesSpecs(Params);
            var Messages = await unitOfWork.Repository<Message>().GetAllWithSpecification(Specs)
                .Select(M => new MessageDTOResponse
                {
                    Id = M.Id,
                    SenderEmail = M.SenderEmail,
                    SenderName = M.SenderName,
                    PhoneNumber = M.PhoneNumber,
                    Body = M.Body,
                    CreatedAt = M.CreatedAt,
                    IsRead = M.IsRead
                })
                .ToListAsync();

            var CountSpecs = new CountAllMessagesSpecs(Params);
            var Count = await unitOfWork.Repository<Message>().GetCountWithSpecs(CountSpecs);

            var Pagination = new Pagination<MessageDTOResponse>(Params.PageNumber, Params.PageSize, Count, Messages);

            return APIResponse<Pagination<MessageDTOResponse>>.SuccessResponse(200, Pagination, "Messages Retrived Successfully");
        }

        public async Task<APIResponse<MessageDTOResponse>> GetById(int id)
        {
            var Message = await unitOfWork.Repository<Message>().GetByIdAsync(id);

            if (Message is null)
                return APIResponse<MessageDTOResponse>.FailureResponse(404, null, "Message Not Found.");

            var MessageDto = new MessageDTOResponse()
            {
                Id = Message.Id,
                SenderName = Message.SenderName,
                SenderEmail = Message.SenderEmail,
                PhoneNumber = Message.PhoneNumber,
                Body = Message.Body,
                CreatedAt = Message.CreatedAt,
                IsRead = Message.IsRead
            };

            return APIResponse<MessageDTOResponse>.SuccessResponse(200, MessageDto, "Message Retrieved Successfully.");
        }
        
        public async Task<APIResponse<string>> Add(MessageDTORequest message)
        {
            var Message = new Message()
            {
                Body = message.Body,
                PhoneNumber = message.PhoneNumber,
                SenderName = message.SenderName,
                SenderEmail = message.SenderEmail
            };

            await unitOfWork.Repository<Message>().AddAsync(Message);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to add message.");

            await hubContext.Clients.All.SendAsync("ReceiveNotification", message.SenderName , message.Body);

            return APIResponse<string>.SuccessResponse(200, null, "Message added successfully.");
        }

        public async Task<APIResponse<string>> Delete(int id)
        {
            var Message = await unitOfWork.Repository<Message>().GetByIdAsync(id);

            if (Message is null)
                return APIResponse<string>.FailureResponse(404, null, "Message Not Found.");

            unitOfWork.Repository<Message>().Delete(Message);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to delete message.");

            return APIResponse<string>.SuccessResponse(200, null, "Message deleted successfully.");
        }

        public async Task<APIResponse<string>> MarkAsRead(int id)
        {
            var Message = await unitOfWork.Repository<Message>().GetByIdAsync(id);

            if (Message is null)
                return APIResponse<string>.FailureResponse(404, null, "Message Not Found.");

            Message.IsRead = true;
            unitOfWork.Repository<Message>().Update(Message);
            var result = await unitOfWork.CompleteAsync();

            if (!result)
                return APIResponse<string>.FailureResponse(500, null, "Failed to mark message as read.");

            return APIResponse<string>.SuccessResponse(200, null, "Message marked as read successfully.");
        }
    }
}
