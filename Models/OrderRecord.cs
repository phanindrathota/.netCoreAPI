using System;

namespace DotNet46ApiExample.Models
{
    public class OrderRecord
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string OriginalMessage { get; set; }
        public DateTime RequestedAtUtc { get; set; }
        public DateTime SavedAtUtc { get; set; }
    }
}
