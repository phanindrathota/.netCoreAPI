using System;
using DotNet46ApiExample.Models;

namespace DotNet46ApiExample.Services
{
    public class OrderMessageFormatter
    {
        public ExternalMessage Format(OrderRecord record)
        {
            return new ExternalMessage
            {
                MessageId = string.Format("ORD-{0}-{1}", record.Id, Guid.NewGuid().ToString("N")),
                CustomerReference = record.CustomerId,
                OrderReference = record.OrderNumber,
                Total = record.Amount,
                IsoCurrencyCode = record.Currency.ToUpperInvariant(),
                Body = string.Format("Order {0} for customer {1} was received and saved as database row {2}.", record.OrderNumber, record.CustomerId, record.Id),
                CreatedAtUtc = DateTime.UtcNow,
                SourceSystem = ".NET Framework 4.6 Web API"
            };
        }
    }
}
