using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        public record TestRequest(string IdToken);
        [HttpPost]
        public async Task<IActionResult> Test([FromBody]TestRequest req)
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                .VerifyIdTokenAsync(req.IdToken);
            string uid = decodedToken.Uid;
            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);

            return Ok(new {Message = "Success"});
        }
    }
}