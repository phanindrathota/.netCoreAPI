using System;

namespace DotNet46ApiExample.Models
{
    public class ExternalMessage
    {
        public string MessageId { get; set; }
        public string CustomerReference { get; set; }
        public string OrderReference { get; set; }
        public decimal Total { get; set; }
        public string IsoCurrencyCode { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string SourceSystem { get; set; }
    }
}
