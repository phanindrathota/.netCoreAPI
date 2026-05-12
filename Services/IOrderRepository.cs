using System.Threading.Tasks;
using DotNet46ApiExample.Models;

namespace DotNet46ApiExample.Services
{
    public interface IOrderRepository
    {
        Task<OrderRecord> SaveAsync(IncomingOrderRequest request);
    }
}
