using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public record TestRequest(string IdToken);
        [HttpPost]
        public IActionResult Test()
        {
            var something = Request;
            return Ok(new {Message = "Success"});
        }
    }
}