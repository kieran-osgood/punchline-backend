using Microsoft.AspNetCore.Mvc;

namespace GraphQL
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new {Message = "Success"});
        }
    }
}