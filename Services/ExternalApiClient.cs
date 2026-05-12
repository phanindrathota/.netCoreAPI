using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNet46ApiExample.Models;
using Newtonsoft.Json;

namespace DotNet46ApiExample.Services
{
    public class ExternalApiClient : IExternalApiClient
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly string _externalApiUrl;

        public ExternalApiClient()
            : this(ConfigurationManager.AppSettings["ExternalApiUrl"])
        {
        }

        public ExternalApiClient(string externalApiUrl)
        {
            _externalApiUrl = externalApiUrl;
        }

        public async Task<string> SendAsync(ExternalMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            using (var response = await Client.PostAsync(_externalApiUrl, content).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                return response.StatusCode.ToString();
            }
        }
    }
}
