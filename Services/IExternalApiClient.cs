using System.Threading.Tasks;
using DotNet46ApiExample.Models;

namespace DotNet46ApiExample.Services
{
    public interface IExternalApiClient
    {
        Task<string> SendAsync(ExternalMessage message);
    }
}
