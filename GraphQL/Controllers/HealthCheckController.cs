using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Index()
        {
            return Ok();
        }    
    }
}
