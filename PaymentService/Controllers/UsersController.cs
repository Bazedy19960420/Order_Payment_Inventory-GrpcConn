using Microsoft.AspNetCore.Mvc;
using PaymentService.Data;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = dbContext.users.ToList();
            return Ok(users);
        }
    }
}