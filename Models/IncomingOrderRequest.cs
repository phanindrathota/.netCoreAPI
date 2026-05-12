using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet46ApiExample.Models
{
    public class IncomingOrderRequest
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string OrderNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        public string Message { get; set; }

        public DateTime? RequestedAtUtc { get; set; }
    }
}
