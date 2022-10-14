using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoC.DistributedAspNetIdentity.Web.Models;

namespace PoC.DistributedAspNetIdentity.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
        }

        [HttpGet("check-session")]
        public async Task<ActionResult<UserModel>> CheckSession()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            //if (!isAuthenticated) return Unauthorized();

            return Ok(new UserModel());
            //return Ok(new UserModel
            //{
            //    Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            //    Email = User.FindFirstValue(ClaimTypes.Email),
            //    Name = User.FindFirstValue(ClaimTypes.Name),
            //    Surname = User.FindFirstValue(ClaimTypes.Surname),
            //});
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login([FromBody] LoginModel request)
        {
            // TODO: Sample, validate credentials and issue cookie
            var valid = request.Email == "admin@admin.com" && request.Password == "admin";
            if (!valid)
            {
                return ValidationProblem(BuildValidationError("InvalidCredentials", "Invalid credentials"));
            }

            return Ok(new UserModel
            {
                Email = request.Email,
            });
        }

        private static ValidationProblemDetails BuildValidationError(string errorCode, string message)
        {
            return new ValidationProblemDetails(
                new Dictionary<string, string[]> { { errorCode, new[] { message } } }
            );
        }
    }
}