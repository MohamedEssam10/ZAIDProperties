using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Message
{
    public class MessageDTORequest
    {
        public string SenderName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? SenderEmail { get; set; } = null!;

        [RegularExpression(@"^(?:(?:\+|00)20|0)1[0125]\d{8}$", ErrorMessage = "Please enter a valid Egyptian phone number.")]
        public string PhoneNumber { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
