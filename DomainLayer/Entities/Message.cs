using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderName { get; set; } = null!;
        public string? SenderEmail { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

    }
}
