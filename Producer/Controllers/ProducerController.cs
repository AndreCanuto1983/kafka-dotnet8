using Microsoft.AspNetCore.Mvc;
using Producer.Models;
using Producer.Services;
using System.Text.Json;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController(ProducerService producerService) : ControllerBase
    {
        private readonly ProducerService _producerService = producerService;

        [HttpPost]
        public async Task<IActionResult> AddToQueue(ProducerRequest producerRequest)
        {
            await _producerService.ProduceAsync("Canuto", JsonSerializer.Serialize(producerRequest));

            return Ok("Data entered successfully.");
        }
    }
}