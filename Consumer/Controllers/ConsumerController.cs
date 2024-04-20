using Consumer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Consumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController(ConsumerService consumerService, ILogger<ConsumerController> logger) : ControllerBase
    {
        private readonly ConsumerService _consumerService = consumerService;
        private readonly ILogger<ConsumerController> _logger = logger;

        [HttpPost]
        public IActionResult ConsumerManualExecution(CancellationToken cancellationToken)
        {
            try
            {
                var data = _consumerService.ProcessKafkaMessage(cancellationToken);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming the queue: {Ex}", ex.Message);
                return StatusCode((int)HttpStatusCode.BadGateway, ex.Message);
            }
        }
    }
}
