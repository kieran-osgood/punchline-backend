using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Path = System.IO.Path;
using SO = System.IO.File;

namespace GraphQL.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class DeepLinkController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public DeepLinkController(IWebHostEnvironment environment) => _environment = environment;

        [HttpGet]
        [Route(".well-known/apple-app-site-association")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Index()
        {
            return Content(
                await SO.ReadAllTextAsync(Path.Combine(_environment.WebRootPath,
                    ".well-known/apple-app-site-association")),
                "text/plain"
            );
        }

        [HttpGet]
        [Route("share/joke")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public RedirectResult RedirectToStore()
        {
            var userAgent = Request.Headers["User-Agent"].ToString().ToLower();
            /**
             * Associated domains will either deep link using the universal link (web_url/share/joke/id)
             * or they will be sent the the web page in their browser - then this method is called
             * so we just direct them to the relevant store
             */
            if (userAgent.Contains("iphone;") || userAgent.Contains("ipad;") || userAgent.Contains("macintosh;"))
                // ! Need to change this to be our production app url
                return new RedirectResult("itms-apps://itunes.apple.com/us/app/imdb-movies-tv/id1567628239");

            if (userAgent.Contains("android"))
                return new RedirectResult("market://details?id=com.ko.punchline");

            return new RedirectResult("404");
        }
    }
}