using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DotNet46ApiExample.Models;

namespace DotNet46ApiExample.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository()
            : this(ConfigurationManager.ConnectionStrings["OrdersDb"].ConnectionString)
        {
        }

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<OrderRecord> SaveAsync(IncomingOrderRequest request)
        {
            var requestedAtUtc = request.RequestedAtUtc ?? DateTime.UtcNow;
            var savedAtUtc = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.SaveIncomingOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerId", request.CustomerId);
                command.Parameters.AddWithValue("@OrderNumber", request.OrderNumber);
                command.Parameters.AddWithValue("@Amount", request.Amount);
                command.Parameters.AddWithValue("@Currency", request.Currency);
                command.Parameters.AddWithValue("@OriginalMessage", (object)request.Message ?? DBNull.Value);
                command.Parameters.AddWithValue("@RequestedAtUtc", requestedAtUtc);
                command.Parameters.AddWithValue("@SavedAtUtc", savedAtUtc);

                await connection.OpenAsync().ConfigureAwait(false);
                var id = Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));

                return new OrderRecord
                {
                    Id = id,
                    CustomerId = request.CustomerId,
                    OrderNumber = request.OrderNumber,
                    Amount = request.Amount,
                    Currency = request.Currency,
                    OriginalMessage = request.Message,
                    RequestedAtUtc = requestedAtUtc,
                    SavedAtUtc = savedAtUtc
                };
            }
        }
    }
}
