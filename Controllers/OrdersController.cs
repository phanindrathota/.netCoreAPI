using System.Threading.Tasks;
using System.Web.Http;
using DotNet46ApiExample.Models;
using DotNet46ApiExample.Services;

namespace DotNet46ApiExample.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderRepository _repository;
        private readonly OrderMessageFormatter _formatter;
        private readonly IKafkaMessageProducer _producer;
        private readonly IExternalApiClient _externalApiClient;

        public OrdersController()
            : this(new OrderRepository(), new OrderMessageFormatter(), new KafkaMessageProducer(), new ExternalApiClient())
        {
        }

        public OrdersController(
            IOrderRepository repository,
            OrderMessageFormatter formatter,
            IKafkaMessageProducer producer,
            IExternalApiClient externalApiClient)
        {
            _repository = repository;
            _formatter = formatter;
            _producer = producer;
            _externalApiClient = externalApiClient;
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(IncomingOrderRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var savedRecord = await _repository.SaveAsync(request).ConfigureAwait(false);
            var formattedMessage = _formatter.Format(savedRecord);
            var kafkaTopic = await _producer.PublishAsync(formattedMessage).ConfigureAwait(false);
            var externalStatus = await _externalApiClient.SendAsync(formattedMessage).ConfigureAwait(false);

            return Ok(new OrderResponse
            {
                DatabaseId = savedRecord.Id,
                KafkaTopic = kafkaTopic,
                ExternalApiStatus = externalStatus,
                MessageId = formattedMessage.MessageId
            });
        }
    }
}
