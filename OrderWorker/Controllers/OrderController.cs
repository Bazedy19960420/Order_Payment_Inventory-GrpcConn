using Microsoft.AspNetCore.Mvc;
using OrderWorker.Models;
using OrderWorker.Services;

namespace OrderWorker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderServices orderService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order,CancellationToken cancellationToken)
        {
            try{
            var orderCreated = await orderService.CreateOrderWithItems(order,cancellationToken);
            return Ok($"your Order cost {orderCreated.TotalPrice}");
            }
            catch(Exception ex)
            {
                return BadRequest($"Erorr Creating Order :{ex.Message}");
            }
        }
    }
}