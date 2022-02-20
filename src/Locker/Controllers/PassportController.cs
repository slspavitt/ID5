using Locker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("passport")]
    [Authorize]
    public class PassportController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new Passport() { pasport_issuer = "UK", passport_number = "123" });
        }
    }
}
    