using InventoryService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetItemsQuantity()
        {
            var items =await dbContext.Items.ToListAsync();
            return Ok(items);
        }
    }
}