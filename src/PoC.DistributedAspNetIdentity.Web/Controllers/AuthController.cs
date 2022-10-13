using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoC.DistributedAspNetIdentity.Web.Models;

namespace PoC.DistributedAspNetIdentity.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
        }

        [HttpGet]
        public bool IsAuthenticated()
        {
            return User.Identity?.IsAuthenticated ?? false;
        }


        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel request)
        {
            // TODO: Sample, validate credentials and issue cookie
            var valid = request.Email == "admin" && request.Password == "admin";
            if (!valid)
            {
                return ValidationProblem(type: "InvalidCredentials");
            }

            return Ok();
        }
    }
}