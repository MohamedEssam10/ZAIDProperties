using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Message
{
    public class MessageDTORequest
    {
        public string SenderName { get; set; } = null!;
        public string? SenderEmail { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
