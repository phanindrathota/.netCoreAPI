using System.Threading.Tasks;
using DotNet46ApiExample.Models;

namespace DotNet46ApiExample.Services
{
    public interface IKafkaMessageProducer
    {
        Task<string> PublishAsync(ExternalMessage message);
    }
}
