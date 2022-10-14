using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoC.DistributedAspNetIdentity.Web.Services;
using PoC.DistributedAspNetIdentity.Web.Services.Dto;

namespace PoC.DistributedAspNetIdentity.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersApiClient _usersApiClient;

        public AuthController(IUsersApiClient usersApiClient)
        {
            _usersApiClient = usersApiClient;
        }

        [HttpGet("check-session")]
        public ActionResult<UserResponse> CheckSession()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            //if (!isAuthenticated) return Unauthorized();

            return Ok(new UserResponse());
            //return Ok(new UserModel
            //{
            //    Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
            //    Email = User.FindFirstValue(ClaimTypes.Email),
            //    Name = User.FindFirstValue(ClaimTypes.Name),
            //    Surname = User.FindFirstValue(ClaimTypes.Surname),
            //});
        }


        [HttpPost("login")]
        public async Task<UserResponse> Login([FromBody] CheckCredentialsRequest request, CancellationToken cancellationToken)
        {
            var response = await _usersApiClient.CheckCredentials(request, cancellationToken);

            // TODO: ISSUE Auth cookie

            return response;
        }
    }
}